﻿using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class IncommeTax : Tax
    {
        private readonly int percentage;

        public IncommeTax(int percentage, int value, SpaceDto spaceDto) : base(value, spaceDto)
        {
            this.percentage = percentage;
        }

        public override void Check(Player player)
        {
            // TODO: ask player for percentage or fix tax.
        }

        public override IEnumerable<string> Text => new[] { "Income Tax", $"(Pay {percentage}% or ${value})" };
    }
}
