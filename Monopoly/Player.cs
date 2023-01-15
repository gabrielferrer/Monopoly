namespace Monopoly
{
    class Player
    {
        private readonly string name;
        private int money;

        public Player(string name)
        {
            this.name = name;
        }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine(nameof(Player));
            stream.WriteLine($"Name: {name}");
            stream.WriteLine($"Money: {money}");
        }
#endif
    }
}
