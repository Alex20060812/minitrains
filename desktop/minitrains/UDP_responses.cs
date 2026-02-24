using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minitrains
{


    internal class UDP_responses
    {
        private static CancellationTokenSource _cts;
        private readonly UdpClient _udpClient;
        
        // Esemény, ha új válasz érkezik (UI-hoz vagy logikához)
        public static event Action<string> OnResponseReceived;
        private readonly string _z21Ip;
        private readonly int _z21Port;

        public UDP_responses(UdpClient udpClient, string z21Ip, int z21Port)
        {
            _udpClient = udpClient;
            _z21Ip = z21Ip;
            _z21Port = z21Port;
            
        }
        
        public void StartListening(string _z21Ip, int _z21Port)
        {
            _cts = new CancellationTokenSource();


            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var result = await _udpClient.ReceiveAsync();
                        MessageBox.Show("valami");
                        InterpretZ21Response(result.Buffer);

                    }
                    catch (ObjectDisposedException)
                    {
                        // UDP kliens leállítva
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }, _cts.Token);
        }

        public static void StopListening()
        {
            _cts?.Cancel();

        }

        public static string InterpretZ21Response(byte[] response)
        {

            if (response[0] == 0x07 && response[2] == 0x40 && response[4] == 0x61 && response[5] == 0x00 && response[6] == 0x61)
            {
                MessageBox.Show("Pálya kikapcs");
                return "Pálya kikapcsolva";

            }
            if (response[0] == 0x07 && response[2] == 0x40 && response[4] == 0x61 && response[5] == 0x01 && response[6] == 0x60)
            {
                MessageBox.Show("pálya be");
                return "Pálya bekapcsolva";
            }

            if (response[0] == 0x07 && response[2] == 0x40 && response[4] == 0x61 && response[5] == 0x08 && response[6] == 0x69)
            {
                return "Zárlat";
            }
            return null;
        }

    }

}
