using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Board
    {
        private List<Spaces.Space> spaces;

        public Board(IEnumerable<Titles.TitleDeed> titleDeeds)
        {
            spaces = new List<Spaces.Space>();

            spaces.Add(new Spaces.Go());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.MediterraneanAvenue)));
            spaces.Add(new Spaces.CommunityChest());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.BalticAvenue)));
            spaces.Add(new Spaces.IncommeTax(10, 200));
            spaces.Add(new Spaces.Railroad(titleDeeds.First(x => x.Name == PropertyNames.ReadingRailroad)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.OrientalAvenue)));
            spaces.Add(new Spaces.Chance());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.VermontAvenue)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.ConnecticutAvenue)));
            spaces.Add(new Spaces.Jail());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.StCharlesPlace)));
            spaces.Add(new Spaces.Utility(titleDeeds.First(x => x.Name == PropertyNames.ElectricCompany)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.StatesAvenue)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.VirginiaAvenue)));
            spaces.Add(new Spaces.Railroad(titleDeeds.First(x => x.Name == PropertyNames.PennsylvaniaRailroad)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.StJamesPlace)));
            spaces.Add(new Spaces.CommunityChest());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.TennesseeAvenue)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.NewYorkAvenue)));
            spaces.Add(new Spaces.Parking());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.KentuckyAvenue)));
            spaces.Add(new Spaces.Chance());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.IndianaAvenue)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.IllinoisAvenue)));
            spaces.Add(new Spaces.Railroad(titleDeeds.First(x => x.Name == PropertyNames.BnORailroad)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.AtlanticAvenue)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.VentnorAvenue)));
            spaces.Add(new Spaces.Utility(titleDeeds.First(x => x.Name == PropertyNames.WaterWorks)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.MarvinGardens)));
            spaces.Add(new Spaces.GoToJail());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.PacificAvenue)));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.NorthCarolinaAvenue)));
            spaces.Add(new Spaces.CommunityChest());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.PennsylvaniaAvenue)));
            spaces.Add(new Spaces.Railroad(titleDeeds.First(x => x.Name == PropertyNames.ShortLine)));
            spaces.Add(new Spaces.Chance());
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.ParkPlace)));
            spaces.Add(new Spaces.LuxuryTax(75));
            spaces.Add(new Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.Boardwalk)));
        }

        public void Clear()
        {
            foreach (var space in spaces) space.Clear();
        }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine(nameof(Board));
            foreach (var space in spaces) space.Log(stream);
        }
#endif

        public IEnumerable<Spaces.Space> Spaces => spaces;
    }
}
