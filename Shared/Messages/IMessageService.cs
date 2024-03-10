using System.Net.Sockets;

namespace Shared.Messages
{
    public interface IMessageService
    {
        Message Read(TcpClient client);

        void Write(TcpClient client, Message message);
    }
}
