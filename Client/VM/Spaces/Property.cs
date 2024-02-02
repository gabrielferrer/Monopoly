using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public abstract class Property : Space
    {
        public Property(Titles.TitleDeed titleDeed, SpaceDto spaceDto) : base(spaceDto)
        {
            TitleDeed = titleDeed;
        }

        public Titles.TitleDeed TitleDeed { get; }

        public override IEnumerable<string> Text => new[] { $"{TitleDeed.Name}", $"$({TitleDeed.Price})" };
    }
}
