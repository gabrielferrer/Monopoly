namespace Monopoly.Cards
{
    class Chance : Card
    {
        public Chance(string text, System.Action<Board, Player> rule) : base(text, rule) { }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine(nameof(Chance));
        }
#endif
    }
}
