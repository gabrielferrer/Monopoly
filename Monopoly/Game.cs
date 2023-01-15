using System;
using System.Collections.Generic;

namespace Monopoly
{
    class Game
    {
        private Board board;
        private List<Titles.TitleDeed> titleDeeds;
        private List<Cards.CommunityChest> communityChest;
        private List<Cards.Chance> chance;
        private List<Player> players;
        private int hotels;
        private int houses;

        public Game()
        {
            players = new List<Player>();

            players.Add(new Player(TokenNames.Cannon));
            players.Add(new Player(TokenNames.Thimble));
            players.Add(new Player(TokenNames.TopHat));
            players.Add(new Player(TokenNames.Iron));
            players.Add(new Player(TokenNames.Battleship));
            players.Add(new Player(TokenNames.Boot));
            players.Add(new Player(TokenNames.RaceCar));
            players.Add(new Player(TokenNames.Dog));
            players.Add(new Player(TokenNames.Rider));
            players.Add(new Player(TokenNames.Wheelbarrow));

            communityChest = new List<Cards.CommunityChest>();

            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.AdvanceToGo, RuleAdvanceToGo));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.BankError, RuleBankError));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.DoctorFees, RuleDoctorFees));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.StockSale, RuleStockSale));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.GetOutOfJailFree, RuleGetOutOfJailFree));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.GoToJail, RuleGoToJail));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.GrandOperaNight, RuleGrandOperaNight));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.HolidayFund, RuleHolidayFund));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.IncomeTaxRefund, RuleIncomeTaxRefund));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.Birthday, RuleBirthday));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.LifeInsurance, RuleLifeInsurance));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.HospitalFees, RuleHospitalFees));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.SchoolFees, RuleSchoolFees));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.ConsultancyFee, RuleConsultancyFee));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.StreetRepairs, RuleStreetRepairs));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.BeautyContest, RuleBeautyContest));
            communityChest.Add(new Cards.CommunityChest(Cards.CommunityChestTexts.Inherit, RuleInherit));

            chance = new List<Cards.Chance>();

            chance.Add(new Cards.Chance(Cards.ChanceTexts.AdvanceToGo, RuleAdvanceToGo));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.AdvanceToIllinois, RuleAdvanceToIllinois));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.AdvanceStCharlesPlace, RuleAdvanceStCharlesPlace));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.AdvanceToNearestUtility, RuleAdvanceToNearestUtility));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.AdvanceToNearestRailroad, RuleAdvanceToNearestRailroad));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.BankPaysDividend, RuleBankPaysDividend));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.GetOutOfJailFree, RuleGetOutOfJailFree));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.GoBackThreeSpaces, RuleGoBackThreeSpaces));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.GoToJail, RuleGoToJail));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.MakeGeneralRepairs, RuleMakeGeneralRepairs));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.TakeTripToReading, RuleTakeTripToReading));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.TakeWalkOnBoardwalk, RuleTakeWalkOnBoardwalk));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.ElectedChairman, RuleElectedChairman));
            chance.Add(new Cards.Chance(Cards.ChanceTexts.LoanMatures, RuleLoanMatures));

            titleDeeds = new List<Titles.TitleDeed>();

            titleDeeds.Add(new Titles.Street(PropertyNames.MediterraneanAvenue, 60, 2, 250, new[] { 10, 30, 90, 160 }, 30, 50, 50));
            titleDeeds.Add(new Titles.Street(PropertyNames.BalticAvenue, 60, 4, 450, new[] { 20, 60, 180, 320 }, 30, 50, 50));
            titleDeeds.Add(new Titles.Railroad(PropertyNames.ReadingRailroad, 200, 25, new[] { 50, 100, 200 }, 100));
            titleDeeds.Add(new Titles.Street(PropertyNames.OrientalAvenue, 100, 6, 50, new[] { 30, 90, 270, 400 }, 550, 50, 50));
            titleDeeds.Add(new Titles.Street(PropertyNames.VermontAvenue, 100, 6, 50, new[] { 30, 90, 270, 400 }, 550, 50, 50));
            titleDeeds.Add(new Titles.Street(PropertyNames.ConnecticutAvenue, 120, 8, 60, new[] { 40, 100, 300, 450 }, 600, 50, 50));
            titleDeeds.Add(new Titles.Street(PropertyNames.StCharlesPlace, 140, 10, 70, new[] { 50, 150, 450, 625 }, 750, 100, 100));
            titleDeeds.Add(new Titles.Utility(PropertyNames.ElectricCompany, 150, 75));
            titleDeeds.Add(new Titles.Street(PropertyNames.StatesAvenue, 140, 10, 70, new[] { 50, 150, 450, 625 }, 750, 100, 100));
            titleDeeds.Add(new Titles.Street(PropertyNames.VirginiaAvenue, 160, 12, 80, new[] { 60, 180, 500, 700 }, 900, 100, 100));
            titleDeeds.Add(new Titles.Railroad(PropertyNames.PennsylvaniaRailroad, 200, 25, new[] { 50, 100, 200 }, 100));
            titleDeeds.Add(new Titles.Street(PropertyNames.StJamesPlace, 180, 14, 90, new[] { 70, 200, 550, 750 }, 950, 100, 100));
            titleDeeds.Add(new Titles.Street(PropertyNames.TennesseeAvenue, 180, 14, 90, new[] { 70, 200, 550, 750 }, 950, 100, 100));
            titleDeeds.Add(new Titles.Street(PropertyNames.NewYorkAvenue, 200, 16, 100, new[] { 80, 220, 600, 800 }, 1000, 100, 100));
            titleDeeds.Add(new Titles.Street(PropertyNames.KentuckyAvenue, 220, 18, 110, new[] { 90, 250, 700, 875 }, 1050, 150, 150));
            titleDeeds.Add(new Titles.Street(PropertyNames.IndianaAvenue, 220, 18, 110, new[] { 90, 250, 700, 875 }, 1050, 150, 150));
            titleDeeds.Add(new Titles.Street(PropertyNames.IllinoisAvenue, 240, 20, 120, new[] { 100, 300, 750, 925 }, 1100, 150, 150));
            titleDeeds.Add(new Titles.Railroad(PropertyNames.BnORailroad, 200, 25, new[] { 50, 100, 200 }, 100));
            titleDeeds.Add(new Titles.Street(PropertyNames.AtlanticAvenue, 260, 22, 130, new[] { 110, 330, 800, 975 }, 1150, 150, 150));
            titleDeeds.Add(new Titles.Street(PropertyNames.VentnorAvenue, 260, 22, 130, new[] { 110, 330, 800, 975 }, 1150, 150, 150));
            titleDeeds.Add(new Titles.Utility(PropertyNames.WaterWorks, 150, 75));
            titleDeeds.Add(new Titles.Street(PropertyNames.MarvinGardens, 280, 24, 140, new[] { 120, 360, 850, 1025 }, 1200, 150, 150));
            titleDeeds.Add(new Titles.Street(PropertyNames.PacificAvenue, 300, 26, 150, new[] { 130, 390, 900, 1100 }, 1275, 200, 200));
            titleDeeds.Add(new Titles.Street(PropertyNames.NorthCarolinaAvenue, 300, 26, 150, new[] { 130, 390, 900, 1100 }, 1275, 200, 200));
            titleDeeds.Add(new Titles.Street(PropertyNames.PennsylvaniaAvenue, 320, 28, 160, new[] { 150, 450, 1000, 1200 }, 1400, 200, 200));
            titleDeeds.Add(new Titles.Railroad(PropertyNames.ShortLine, 200, 25, new[] { 50, 100, 200 }, 100));
            titleDeeds.Add(new Titles.Street(PropertyNames.ParkPlace, 350, 35, 175, new[] { 175, 500, 1100, 1300 }, 1500, 200, 200));
            titleDeeds.Add(new Titles.Street(PropertyNames.Boardwalk, 400, 50, 200, new[] { 200, 600, 1400, 1700 }, 2000, 200, 200));

            board = new Board(titleDeeds);
            hotels = 12;
            houses = 32;
        }

        #region Rules

        private void RuleInherit(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBeautyContest(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleStreetRepairs(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleConsultancyFee(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleSchoolFees(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleHospitalFees(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleLifeInsurance(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBirthday(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleIncomeTaxRefund(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleHolidayFund(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGrandOperaNight(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleStockSale(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleDoctorFees(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBankError(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleLoanMatures(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleElectedChairman(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleTakeWalkOnBoardwalk(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleTakeTripToReading(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleMakeGeneralRepairs(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGoToJail(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGoBackThreeSpaces(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGetOutOfJailFree(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBankPaysDividend(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToNearestRailroad(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToNearestUtility(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceStCharlesPlace(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToIllinois(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToGo(Board board, Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        #endregion

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            foreach (var titleDeed in titleDeeds) titleDeed.Log(stream);
            foreach (var card in communityChest) card.Log(stream);
            foreach (var card in chance) card.Log(stream);
            foreach (var player in players) player.Log(stream);
            board.Log(stream);
            stream.WriteLine($"Hotels: {hotels}");
            stream.WriteLine($"Houses: {houses}");
        }
#endif

        public Board Board => board;
    }
}
