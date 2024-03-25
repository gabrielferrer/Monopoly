using ProtoBuf;

namespace Shared.Messages
{
    [ProtoContract]
    public class GameCreatedMessage : Message
    {
        public GameCreatedMessage()
        {
        }

        public GameCreatedMessage(GameCreatedMessage message)
        {
        }

        public override Message Clone()
        {
            return new GameCreatedMessage(this);
        }
    }
}
