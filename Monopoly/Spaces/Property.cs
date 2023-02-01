using System.Collections.Generic;

namespace Monopoly.Spaces
{
    public abstract class Property : Space
    {
        protected readonly Titles.TitleDeed titleDeed;

        public Property(Titles.TitleDeed titleDeed)
        {
            this.titleDeed = titleDeed;
        }

        public override IEnumerable<string> Text => new[] { $"{titleDeed.Name}", $"$({titleDeed.Price})" };
    }
}
