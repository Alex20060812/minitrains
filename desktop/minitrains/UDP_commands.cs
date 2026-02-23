using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minitrains
{
    internal class UDP_commands
    {
        private readonly string _z21Ip;
        private readonly int _z21Port;

        public UDP_commands(string z21Ip, int z21Port)
        {
            _z21Ip = z21Ip;
            _z21Port = z21Port;
        }
        UDP_connection _udpConnection = new UDP_connection();
        public void Track_ON()
        {
            byte[] _request = new byte[]
           {
            0x07, 0x00, 0x40, 0x00, 0x21, 0x81, 0xa0
           };
            _udpConnection.SendToZ21(_request, _z21Ip, _z21Port);
        }
        public void Track_OFF()
        {
            byte[] _request = new byte[]
            {
               0x07, 0x00, 0x40, 0x00, 0x21, 0x80, 0xa1
            };

            _udpConnection.SendToZ21(_request, _z21Ip, _z21Port);
        }
        public void Setup()
        {
            byte[] _request = new byte[]
            {
                0x08, 0x00, 0x50, 0x00, // DataLen, ?, Header, ?
                0x01, 0x00, 0x00, 0x00  // Broadcast-Flags: csak az első flag bekapcsolva (little endian)
            };
            _udpConnection.SendToZ21(_request, _z21Ip, _z21Port);
        }
    }
}
