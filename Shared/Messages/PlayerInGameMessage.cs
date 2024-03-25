namespace Shared.Messages
{
    public class PlayerInGameMessage : Message
    {
        public PlayerInGameMessage()
        {
        }

        public PlayerInGameMessage(PlayerInGameMessage message)
        {
        }

        public override Message Clone()
        {
            return new PlayerInGameMessage(this);
        }
    }
}
