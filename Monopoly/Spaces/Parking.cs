using System.Collections.Generic;

namespace Monopoly.Spaces
{
    class Parking : Space
    {
        public override IEnumerable<string> Text => new[] { "Free Parking" };
    }
}
