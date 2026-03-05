using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class Z21Udp
{
    private UdpClient _udp;
    private IPEndPoint _z21EndPoint;
    private CancellationTokenSource _cts;

    public bool IsConnected { get; private set; }

    // UI események
    public event Action<bool> OnTrackPowerChanged;
    public event Action<int, int, bool> OnLocoInfo;
    public event Action<string> OnLog;

    // ÚJ: kapcsolat állapot esemény
    public event Action<bool> OnConnectionStateChanged;

    // ÚJ: utolsó version-reply ideje
    private DateTime _lastVersionReply = DateTime.MinValue;

    public void Connect(string ip, int localPort = 0)
    {
        _udp = new UdpClient(localPort);
        _z21EndPoint = new IPEndPoint(IPAddress.Parse(ip), 21105);

        _cts = new CancellationTokenSource();

        // FONTOS: háttérben, nem blokkolva fusson
        _ = StartListeningAsync(_cts.Token);

        IsConnected = true;

        SetBroadcastFlags();
        RequestSystemState();

        // ÚJ: version ping task indítása
        // ha van ilyen metódusod, azt is async loopként indítsd:
        // _ = StartVersionMonitor(_cts.Token);

        OnLog?.Invoke("Connected to Z21");
        OnConnectionStateChanged?.Invoke(true);
    }

    public void Disconnect()
    {
        _cts?.Cancel();
        _udp?.Close();
        IsConnected = false;
        OnConnectionStateChanged?.Invoke(false);
    }

    // ==========================
    // ===== CONNECT ===========
    // ==========================

    private async Task StartListeningAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                UdpReceiveResult result;
                try
                {
                    result = await _udp.ReceiveAsync().ConfigureAwait(false);
                }
                catch (ObjectDisposedException)
                {
                    // UDP lezárva
                    break;
                }
                catch (SocketException)
                {
                    // hálózati hiba – logolhatod, majd folytathatod
                    continue;
                }
                catch (OperationCanceledException)
                {
                    // token cancel
                    break;
                }

                try
                {
                    ParsePacket(result.Buffer);
                }
                catch
                {
                    // parser hiba lenyelése / log
                }
            }
        }
        finally
        {
            // opcionális: itt jelezheted, hogy a listener leállt
        }
    }

    private void Send(byte[] data)
    {
        if (!IsConnected) return;
        _udp.Send(data, data.Length, _z21EndPoint);
    }

    // ==========================
    // ===== BROADCAST FLAG =====
    // ==========================

    public void SetBroadcastFlags()
    {
        uint flags = 0x00000001;

        byte[] packet = new byte[8];
        packet[0] = 0x08;
        packet[1] = 0x00;
        packet[2] = 0x50;
        packet[3] = 0x00;

        BitConverter.GetBytes(flags).CopyTo(packet, 4);

        Send(packet);
    }

    // ==========================
    // ===== TRACK POWER ========
    // ==========================

    public void TrackPowerOn()
    {
        Send(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x21, 0x81, 0xA0 });
    }

    public void TrackPowerOff()
    {
        Send(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x21, 0x80, 0xA1 });
    }

    public void EmergencyStop()
    {
        Send(new byte[] { 0x06, 0x00, 0x40, 0x00, 0x80, 0x80 });
    }

    // ==========================
    // ===== LOCO DRIVE =========
    // ==========================
    static readonly byte[] dcc28Table =
{
    0x00, // stop
    0x01, // estop
    0x02,0x12,0x03,0x13,
    0x04,0x14,0x05,0x15,
    0x06,0x16,0x07,0x17,
    0x08,0x18,0x09,0x19,
    0x0A,0x1A,0x0B,0x1B,
    0x0C,0x1C,0x0D,0x1D,
    0x0E,0x1E,0x0F,0x1F
};
    public void SetLocoDrive(int address, int step, bool forward)
    {
        if (step < 0) step = 0;
        if (step > 29) step = 29;

        byte adrMsb = (byte)((address >> 8) & 0x3F);
        byte adrLsb = (byte)(address & 0xFF);

        if (address >= 128)
            adrMsb |= 0xC0;
        
        else if (step > 0)
        {
            step += 1;
        }
        byte speedByte = dcc28Table[step];

        if (forward)
            speedByte |= 0x80;   // direction bit (R)

        byte xor = (byte)(0xE4 ^ 0x12 ^ adrMsb ^ adrLsb ^ speedByte);

        byte[] packet =
        {
        0x0A,0x00,
        0x40,0x00,
        0xE4,
        0x12,
        adrMsb,
        adrLsb,
        speedByte,
        xor
    };

        Send(packet);
    }

    // ==========================
    // ===== FUNCTIONS F0-F28 ===
    // ==========================

    public void SetFunction(int address, int functionNumber, bool on)
    {
        byte adrMsb = (byte)((address >> 8) & 0x3F);
        byte adrLsb = (byte)(address & 0xFF);

        if (address >= 128)
            adrMsb |= 0xC0;

        byte type = on ? (byte)0x40 : (byte)0x00;
        byte func = (byte)(type | functionNumber);

        byte xor = (byte)(0xE4 ^ 0xF8 ^ adrMsb ^ adrLsb ^ func);

        byte[] packet = {
            0x0A,0x00,
            0x40,0x00,
            0xE4,
            0xF8,
            adrMsb,
            adrLsb,
            func,
            xor
        };

        Send(packet);
    }

    // ==========================
    // ===== REQUEST STATE ======
    // ==========================

    public void RequestSystemState()
    {
        Send(new byte[] { 0x04, 0x00, 0x85, 0x00 });
    }

    // ==========================
    // ===== PACKET PARSER ======
    // ==========================

    private void ParsePacket(byte[] data)
    {
        if (data.Length < 5) return;

        if (data[2] == 0x40)
        {
            byte xHeader = data[4];

            switch (xHeader)
            {
                case 0x61:
                    HandleBroadcast(data);
                    break;

                case 0xEF:
                    HandleLocoInfo(data);
                    break;

                // ÚJ: LAN_X_GET_VERSION válasz
                
            }
        }
    }

    // ÚJ: version reply kezelése
    

    // ==========================
    // ===== HANDLE BROADCAST ===
    // ==========================

    private void HandleBroadcast(byte[] data)
    {
        byte type = data[5];

        if (type == 0x00)
        {
            OnTrackPowerChanged?.Invoke(false);
            OnLog?.Invoke("Track Power OFF");
        }

        if (type == 0x01)
        {
            OnTrackPowerChanged?.Invoke(true);
            OnLog?.Invoke("Track Power ON");
        }

        if (type == 0x08)
        {
            OnLog?.Invoke("Short Circuit!");
        }
    }

    // ==========================
    // ===== HANDLE LOCO INFO ===
    // ==========================

    private void HandleLocoInfo(byte[] data)
    {
        byte adrMsb = data[5];
        byte adrLsb = data[6];

        int address = ((adrMsb & 0x3F) << 8) + adrLsb;

        byte speedDir = data[8];
        bool forward = (speedDir & 0x80) != 0;
        int speed = speedDir & 0x7F;

        OnLocoInfo?.Invoke(address, speed, forward);
    }
    // ==========================
    // ======EMERGENCY STOP =====
    // ==========================

    public void EmergencyStopLoco(int address, bool forward)
    {
        

        byte adrMsb = (byte)((address >> 8) & 0x3F);
        byte adrLsb = (byte)(address & 0xFF);

        if (address >= 128)
            adrMsb |= 0xC0;
        int step = 1;
        byte speedByte = dcc28Table[step];

        if (forward)
            speedByte |= 0x80;   // direction bit (R)

        byte xor = (byte)(0xE4 ^ 0x12 ^ adrMsb ^ adrLsb ^ speedByte);

        byte[] packet =
        {
        0x0A,0x00,
        0x40,0x00,
        0xE4,
        0x12,
        adrMsb,
        adrLsb,
        speedByte,
        xor
    };

        Send(packet);
    }
}
