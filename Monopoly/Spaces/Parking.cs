using System.Collections.Generic;

namespace Monopoly.Spaces
{
    public class Parking : Space
    {
        public override IEnumerable<string> Text => new[] { "Free Parking" };
    }
}
