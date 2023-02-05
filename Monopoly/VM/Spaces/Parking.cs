using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class Parking : Space
    {
        public Parking(SpaceDto spaceDto) : base(spaceDto) { }

        public override IEnumerable<string> Text => new[] { "Free Parking" };
    }
}
