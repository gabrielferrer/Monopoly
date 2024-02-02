namespace Monopoly.VM.Spaces
{
    public abstract class Tax : Space
    {
        protected readonly int value;

        public Tax(int value, SpaceDto spaceDto) : base(spaceDto)
        {
            this.value = value;
        }
    }
}
