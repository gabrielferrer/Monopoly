using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class Go : Space
    {
        private int salary;

        public Go(int salary, SpaceDto spaceDto) : base(spaceDto) { }

        public override void Check(Player player)
        {
            player.Money += salary;
        }

        public override IEnumerable<string> Text => new[] { "Collect $200 salary as you pass Go", "←" };
    }
}
