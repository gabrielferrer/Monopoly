using System.Linq;

namespace Monopoly
{
    public static class TokenNames
    {
        public const string Cannon = "Cannon";
        public const string Thimble = "Thimble";
        public const string TopHat = "Top Hat";
        public const string Iron = "Iron";
        public const string Battleship = "Battleship";
        public const string Boot = "Boot";
        public const string RaceCar = "Race Car";
        public const string Dog = "Dog";
        public const string Rider = "Rider";
        public const string Wheelbarrow = "Wheelbarrow";
        public const string Airplane = "Airplane";
        public const string Train = "Train";
        public const string Bathtub = "Bathtub";
        public const string Lantern = "Lantern";

        public static string[] AllTokens()
        {
            return typeof(TokenNames).GetFields().Select(x => (string)x.GetValue(null)).ToArray();
        }

        public static int TotalTokens()
        {
            return AllTokens().Length;
        }
    }
}
