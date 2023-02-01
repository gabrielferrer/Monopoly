namespace Monopoly
{
    public static class GameConstants
    {
        public const int MinimumPlayers = 2;
        public static readonly int MaximumPlayers = typeof(TokenNames).GetFields().Length;
        public const int TotalHotels = 12;
        public const int TotalHouses = 32;
    }
}
