namespace Monopoly.Titles
{
    public class Street : TitleDeed
    {
        private readonly int[] houseRent;
        private readonly int hotelRent;
        private readonly int houseCost;
        private readonly int hotelCost;

        public Street(string name, int price, int rent, int mortgage, int[] houseRent, int hotelRent, int houseCost, int hotelCost)
            : base(name, price, rent, mortgage)
        {
            this.houseRent = houseRent;
            this.hotelRent = hotelRent;
            this.houseCost = houseCost;
            this.hotelCost = hotelCost;
        }

#if DEBUG
        public override void Log(System.IO.StreamWriter stream)
        {
            base.Log(stream);
            stream.Write($"House rent: ");

            for (int index = 0; index < houseRent.Length; index++)
            {
                if (index + 1 == houseRent.Length)
                {
                    stream.WriteLine(houseRent[index]);
                }
                else
                {
                    stream.Write(houseRent[index]);
                }

                if (index + 1 < houseRent.Length) stream.Write(", ");
            }

            stream.WriteLine($"Hotel rent: {hotelRent}");
            stream.WriteLine($"House cost: {houseCost}");
            stream.WriteLine($"Hotel cost: {hotelCost}");
        }
#endif
    }
}
