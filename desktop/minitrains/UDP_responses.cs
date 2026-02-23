using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace minitrains
{


    public static class UDP_responses
    {
        private static CancellationTokenSource _cts;
        private static UdpClient _udpClient;

        // Esemény, ha új válasz érkezik (UI-hoz vagy logikához)
        public static event Action<string> OnResponseReceived;

        public static void StartListening(int listenPort)
        {
            _cts = new CancellationTokenSource();
            _udpClient = new UdpClient(listenPort);

            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var result = await _udpClient.ReceiveAsync();
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
            _udpClient?.Close();
        }

        public static string InterpretZ21Response(byte[] response)
        {

            byte length = response[0];
            byte header = response[2];
            byte xHeader = response[4];
            byte data1 = response[6];


            if (header == 0x40 && xHeader == 0x21)
                return "Z21 státusz válasz";
            if (header == 0x40 && xHeader == 0x81)
                return "Z21 rendszer információ válasz";
            if (header == 0x40 && xHeader == 0x24)
                return "Z21 speciális válasz";

            return "Ismeretlen vagy nem kezelt válasz";
        }
    }

}
