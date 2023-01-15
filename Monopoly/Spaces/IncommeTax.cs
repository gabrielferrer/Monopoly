using System.Collections.Generic;

namespace Monopoly.Spaces
{
    class IncommeTax : Tax
    {
        private readonly int percentage;

        public IncommeTax(int percentage, int value) : base(value)
        {
            this.percentage = percentage;
        }

        public override IEnumerable<string> Text => new[] { "Income Tax", $"(Pay {percentage}% or ${value})" };
    }
}
