using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class GoToJail : Space
    {
        public GoToJail(SpaceDto spaceDto) : base(spaceDto) { }

        public override void Check(Player player)
        {
            // TODO:
        }

        public override IEnumerable<string> Text => new[] { "Go To Jail" };
    }
}
