using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class Go : Space
    {
        public Go(SpaceDto spaceDto) : base(spaceDto) { }

        public override IEnumerable<string> Text => new[] { "Collect $200 salary as you pass Go", "←" };
    }
}
