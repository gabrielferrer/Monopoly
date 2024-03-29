﻿namespace Monopoly.Cards
{
    public class CommunityChest : Card
    {
        public CommunityChest(string text, System.Action<VM.Player> rule) : base(text, rule) { }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine(nameof(CommunityChest));
        }
#endif
    }
}
