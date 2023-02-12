﻿using System.Collections.Generic;

namespace Monopoly.VM.Spaces
{
    public class Jail : Space
    {
        public Jail(SpaceDto spaceDto) : base(spaceDto) { }

        public override void Check(Player player)
        {
            // TODO:
        }

        public override IEnumerable<string> Text => new[] { "In Jail/Just Visiting" };
    }
}
