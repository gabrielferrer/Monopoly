using System.Collections.Generic;

namespace Server.Models
{
    class Game
    {
        public Dictionary<ClientKey, Player> Players { get; set; } = new Dictionary<ClientKey, Player>();
    }
}
