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
            var messageCarrierStream = new MemoryStream();
            int bytesRead;

            do
            {
                bytesRead = stream.Read(buffer, 0, TcpCientBufferSize);
                messageCarrierStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead == TcpCientBufferSize);

            if (messageCarrierStream.Length == 0)
            {
                return null;
            }

            messageCarrierStream.Position = 0;

            var messageCarrier = Serializer.Deserialize<MessageCarrier>(messageCarrierStream);
            var messageStream = new MemoryStream(messageCarrier.Message);
            return Serializer.Deserialize(messageCarrier.MessageType, messageStream) as Message;
        }

        public void Write(TcpClient client, Message message)
        {
            var serializedMessage = new MemoryStream();
            Serializer.Serialize(serializedMessage, message);

            var messageCarrier = new MessageCarrier
            {
                MessageType = message.GetType(),
                Message = serializedMessage.ToArray()
            };

            var messageCarrierStream = new MemoryStream();
            Serializer.Serialize(messageCarrierStream, messageCarrier);

            var stream = client.GetStream();
            stream.Write(messageCarrierStream.ToArray(), 0, (int)messageCarrierStream.Length);
        }
    }
}
