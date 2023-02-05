using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class LuxuryTax : Tax
    {
        public LuxuryTax(int value, SpaceDto spaceDto) : base(value, spaceDto) { }

        public override IEnumerable<string> Text => new[] { "Luxury Tax", $"(Pay ${value})" };
    }
}
