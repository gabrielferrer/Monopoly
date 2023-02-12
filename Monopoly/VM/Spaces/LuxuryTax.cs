using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class LuxuryTax : Tax
    {
        public LuxuryTax(int value, SpaceDto spaceDto) : base(value, spaceDto) { }

        public override void Check(Player player)
        {
            if (value > player.Money)
            {
                // TODO: player can't pay.
            }

            player.Money -= value;
        }

        public override IEnumerable<string> Text => new[] { "Luxury Tax", $"(Pay ${value})" };
    }
}
