using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class Parking : Space
    {
        public Parking(SpaceDto spaceDto) : base(spaceDto) { }

        public override void Check(Player player)
        {
            // Nothing to do here.
        }

        public override IEnumerable<string> Text => new[] { "Free Parking" };
    }
}
