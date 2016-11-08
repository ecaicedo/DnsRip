using DnsRip.Exceptions;
using DnsRip.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DnsRip.Utilites
{
    public class SocketHelper : IDisposable
    {
        public SocketHelper(DnsRequest request, string server, TimeSpan timeout)
        {
            try
            {
                IPAddress ip;
                IPAddress.TryParse(server, out ip);

                if (ip == null)
                {
                    IPHostEntry hostEntry = Dns.GetHostEntryAsync(server).GetAwaiter().GetResult();
                    IPAddress.TryParse(hostEntry.AddressList[0].ToString(), out ip);

                }

                _server = new IPEndPoint(ip, 53);
            }
            catch
            {
                throw new InvalidNameserverException(server);
            }

            _request = request;
            _timeout = timeout;
        }

        private Socket _socket;
        private readonly IPEndPoint _server;
        private readonly DnsRequest _request;
        private readonly TimeSpan _timeout;

        public byte[] Send()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, (int)_timeout.TotalMilliseconds);
            _socket.SendTo(_request.Data, _server);

            var buffer = new byte[512];
            var received = _socket.Receive(buffer);
            var data = new byte[received];

            Array.Copy(buffer, data, received);

            return data;
        }

        public void Dispose()
        {
            _socket.Dispose();
        }
    }
}