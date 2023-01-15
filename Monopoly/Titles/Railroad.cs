namespace Monopoly.Titles
{
    class Railroad : TitleDeed
    {
        private readonly int[] rents;

        /// <param name="rent">Rent for one railroad.</param>
        /// <param name="rents">Rents for any other extra railroad (up to four).</param>
        public Railroad(string name, int price, int rent, int[] rents, int mortgage) : base(name, price, rent, mortgage)
        {
            this.rents = rents;
        }

#if DEBUG
        public override void Log(System.IO.StreamWriter stream)
        {
            base.Log(stream);
            stream.Write($"Rents: ");

            for (int index = 0; index < rents.Length; index++)
            {
                if (index + 1 == rents.Length)
                {
                    stream.WriteLine(rents[index]);
                }
                else
                {
                    stream.Write(rents[index]);
                }

                if (index + 1 < rents.Length) stream.Write(", ");
            }
        }
#endif
    }
}
