using ProtoBuf;

namespace Shared.Messages
{
    [ProtoContract]
    [ProtoInclude(1, typeof(CreateGameMessage))]
    [ProtoInclude(2, typeof(GameCreatedMessage))]
    public abstract class Message
    {
        public Message()
        {
        }

        public abstract Message Clone();
    }
}
