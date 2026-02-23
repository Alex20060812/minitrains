using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace minitrains
{
    internal class UDP_3sec
    {
        private readonly UDP_connection _udpConnection = new UDP_connection();
        
        private readonly byte[] _request = new byte[]
        {
            0x07, 0x00, 0x40, 0x00, 0x21, 0x24, 0x05
        };
        private readonly string _z21Ip;
        private readonly int _z21Port;
        private CancellationTokenSource _cts;
        public UDP_3sec(string z21Ip, int z21Port)
        {
            _z21Ip = z21Ip;
            _z21Port = z21Port;
        }
        public void Start()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => SendLoop(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
        }

        private async Task SendLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _udpConnection.SendToZ21(_request, _z21Ip, _z21Port);
                await Task.Delay(3000, token);
            }
        }
    }
}
