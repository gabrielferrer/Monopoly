using System.Collections.Generic;

namespace Monopoly.Spaces
{
    public class Go : Space
    {
        public override IEnumerable<string> Text => new[] { "Collect $200 salary as you pass Go", "←" };
    }
}
