using ProtoBuf;

namespace Shared.Messages
{
    [ProtoContract]
    public class CreateGameMessage : Message
    {
        public CreateGameMessage()
        {
        }

        public CreateGameMessage(CreateGameMessage message)
        {
        }

        public override Message Clone()
        {
            return new CreateGameMessage(this);
        }
    }
}
