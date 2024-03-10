using ProtoBuf;
using System.IO;
using System.Net.Sockets;

namespace Shared.Messages
{
    public class MessageService : IMessageService
    {
        #region Constants

        private const int TcpCientBufferSize = 2048;

        #endregion

        public Message Read(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[TcpCientBufferSize];
            var memoryStream = new MemoryStream();
            int bytesRead;

            do
            {
                bytesRead = stream.Read(buffer, 0, TcpCientBufferSize);
                memoryStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead < TcpCientBufferSize);

            return memoryStream.Length > 0
                ? Serializer.Deserialize<Message>(memoryStream)
                : null;
        }

        public void Write(TcpClient client, Message message)
        {
            var memoryStream = new MemoryStream();
            Serializer.Serialize(memoryStream, message);
            var stream = client.GetStream();
            stream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
        }
    }
}
