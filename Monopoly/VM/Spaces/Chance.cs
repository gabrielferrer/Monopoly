﻿using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class Chance : Space
    {
        public Chance(SpaceDto spaceDto) : base(spaceDto) { }

        public override void Check(Player player)
        {
            // TODO:
        }

        public override IEnumerable<string> Text => new[] { "Chance" };
    }
}
