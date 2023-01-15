namespace Monopoly.Spaces
{
    abstract class Tax : Space
    {
        protected readonly int value;

        public Tax(int value)
        {
            this.value = value;
        }
    }
}
