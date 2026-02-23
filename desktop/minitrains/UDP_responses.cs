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
        private static UdpClient _udpClient;
        
        // Esemény, ha új válasz érkezik (UI-hoz vagy logikához)
        public static event Action<string> OnResponseReceived;
        private readonly string _z21Ip;
        private readonly int _z21Port;

        public UDP_responses(string z21Ip, int z21Port)
        {
            _z21Ip = z21Ip;
            _z21Port = z21Port;
        }
        
    }

}
