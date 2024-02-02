namespace Monopoly.VM.Spaces
{
    public class Street : Property
    {
        public Street(Titles.TitleDeed titleDeed, SpaceDto spaceDto) : base(titleDeed, spaceDto) { }

        public override void Check(Player player)
        {
            // TODO:
        }
    }
}
