using System.Collections.Generic;

namespace Monopoly.Spaces
{
    public class Jail : Space
    {
        public override IEnumerable<string> Text => new[] { "In Jail/Just Visiting" };
    }
}
