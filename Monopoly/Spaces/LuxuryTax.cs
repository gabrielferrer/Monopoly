using System.Collections.Generic;

namespace Monopoly.Spaces
{
    class LuxuryTax : Tax
    {
        public LuxuryTax(int value) : base(value) { }

        public override IEnumerable<string> Text => new[] { "Luxury Tax", $"(Pay ${value})" };
    }
}
