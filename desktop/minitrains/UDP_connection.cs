using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace minitrains
{
    internal class UDP_connection
    {
        public UdpClient udpClient = new UdpClient();
        public void SendToZ21(byte[] data, string ipAddress, int port)
        {
            
            
                var endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                udpClient.Send(data, data.Length, endpoint);
            
        }
    }
}
