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

    public async Task Connect(string ip, int localPort = 0)
    {
        _udp = new UdpClient(localPort);
        _z21EndPoint = new IPEndPoint(IPAddress.Parse(ip), 21105);

        _cts = new CancellationTokenSource();
        StartListening(_cts.Token);

        IsConnected = true;

        SetBroadcastFlags();
        RequestSystemState();

        // ÚJ: version ping task indítása
        //StartVersionMonitor(_cts.Token);

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

    private async void StartListening(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var result  = _udp.ReceiveAsync();
                ParsePacket(result.Result.Buffer);
            }
            catch { }
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

    public void SetLocoDrive(int address, int step, bool forward)
    {
        // step:
        // 0 = STOP
        // 1 = E-STOP
        // 2-29 = Step 1-28

        if (step < 0) step = 0;
        if (step > 29) step = 29;

        byte adrMsb = (byte)((address >> 8) & 0x3F);
        byte adrLsb = (byte)(address & 0xFF);

        if (address >= 128)
            adrMsb |= 0xC0;

        byte speedByte;

        if (step == 0)
        {
            speedByte = 0x00; // STOP
        }
        else if (step == 1)
        {
            speedByte = 0x01; // E-STOP
        }
        else
        {
            int realStep = step - 1; // 1-28

            int low4 = (realStep + 1) / 2;  // 1-14
            bool v5 = (realStep % 2) == 0;  // páros lépés

            speedByte = (byte)(low4 & 0x0F);

            if (v5)
                speedByte |= 0x10; // V5 bit
        }

        if (forward)
            speedByte |= 0x80; // direction

        byte xor = (byte)(0xE4 ^ 0x12 ^ adrMsb ^ adrLsb ^ speedByte);

        byte[] packet = {
        0x0A,0x00,
        0x40,0x00,
        0xE4,
        0x12,      // 28 step mód
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

    public void EmergencyStopLoco(int address)
    {
        byte adrMsb = (byte)((address >> 8) & 0x3F);
        byte adrLsb = (byte)(address & 0xFF);

        if (address >= 128)
            adrMsb |= 0xC0;

        byte xor = (byte)(0x92 ^ adrMsb ^ adrLsb);

        byte[] packet ={
            0x08,0x00,
            0x40,0x00,
            0x92,
            adrMsb,
            adrLsb,
            xor
        };

        Send(packet);
    }
}