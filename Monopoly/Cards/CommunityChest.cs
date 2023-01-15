namespace Monopoly.Cards
{
    class CommunityChest : Card
    {
        public CommunityChest(string text, System.Action<Board, Player> rule) : base(text, rule) { }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine(nameof(CommunityChest));
        }
#endif
    }
}
