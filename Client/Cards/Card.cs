namespace Monopoly.Cards
{
    public abstract class Card
    {
        protected readonly string text;
        protected System.Action<VM.Player> rule;

        public Card(string text, System.Action<VM.Player> rule)
        {
            this.text = text;
            this.rule = rule;
        }
    }
}
