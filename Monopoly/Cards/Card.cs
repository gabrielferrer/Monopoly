namespace Monopoly.Cards
{
    public abstract class Card
    {
        protected readonly string text;
        protected System.Action<Board, Player> rule;

        public Card(string text, System.Action<Board, Player> rule)
        {
            this.text = text;
            this.rule = rule;
        }
    }
}
