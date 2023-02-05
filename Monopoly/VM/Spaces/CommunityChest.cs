using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class CommunityChest : Space
    {
        public CommunityChest(SpaceDto spaceDto) : base(spaceDto) { }

        public override IEnumerable<string> Text => new[] { "Community Chest" };
    }
}
