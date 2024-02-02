namespace Monopoly.Titles
{
    public abstract class TitleDeed
    {
        protected readonly string name;
        protected readonly int price;
        protected readonly int mortgage;
        protected readonly int rent;

        public TitleDeed(string name, int price, int rent, int mortgage)
        {
            this.name = name;
            this.price = price;
            this.rent = rent;
            this.mortgage = mortgage;
        }

        public string Name => name;

        public int Price => price;

#if DEBUG
        public virtual void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine($"Name: {name}");
            stream.WriteLine($"Price: {price}");
            stream.WriteLine($"Mortgage: {mortgage}");
            stream.WriteLine($"Rent: {rent}");
        }
#endif
    }
}
