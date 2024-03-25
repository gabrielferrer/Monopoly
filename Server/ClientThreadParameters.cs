using System.Net.Sockets;

namespace Server
{
    class ClientThreadParameters
    {
        public TcpClient Client { get; set; }

        public ClientKey ClientKey { get; set; }
    }
}
