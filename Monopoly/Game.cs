using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Game
    {
        #region Fields

        private List<Titles.TitleDeed> titleDeeds;
        private List<Cards.CommunityChest> communityChest;
        private List<Cards.Chance> chance;
        private List<VM.Player> players;
        private int hotels;
        private int houses;
        private VM.Player currentPlayer;
        private int firstDie;
        private int secondDie;
        private int currentPlayerIndex;

        #endregion

        #region Events

        public delegate void DiceThrownHandler(object sender, Events.DiceThrownArgs args);

        public event DiceThrownHandler DiceThrown;

        #endregion

        #region Constructors

        public Game()
        {
            CurrentPlayers = new List<VM.Player>();
            players = new List<VM.Player>();

            players.Add(new VM.Player(TokenNames.Cannon, UI.PlayerColors.LightRed));
            players.Add(new VM.Player(TokenNames.Thimble, UI.PlayerColors.Ocher));
            players.Add(new VM.Player(TokenNames.TopHat, UI.PlayerColors.Lime));
            players.Add(new VM.Player(TokenNames.Iron, UI.PlayerColors.Cyan));
            players.Add(new VM.Player(TokenNames.Battleship, UI.PlayerColors.Violet));
            players.Add(new VM.Player(TokenNames.Boot, UI.PlayerColors.Orange));
            players.Add(new VM.Player(TokenNames.RaceCar, UI.PlayerColors.Teal));
            players.Add(new VM.Player(TokenNames.Dog, UI.PlayerColors.Lila));
            players.Add(new VM.Player(TokenNames.Rider, UI.PlayerColors.Yellow));
            players.Add(new VM.Player(TokenNames.Wheelbarrow, UI.PlayerColors.LightBlue));
            players.Add(new VM.Player(TokenNames.Airplane, UI.PlayerColors.DarkPink));
            players.Add(new VM.Player(TokenNames.Train, UI.PlayerColors.Green));
            players.Add(new VM.Player(TokenNames.Bathtub, UI.PlayerColors.Red));
            players.Add(new VM.Player(TokenNames.Lantern, UI.PlayerColors.Blue));

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

            Board = new Board(titleDeeds);
            hotels = GameConstants.TotalHotels;
            houses = GameConstants.TotalHouses;
        }

        #endregion

        #region Rules

        private void RuleInherit(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBeautyContest(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleStreetRepairs(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleConsultancyFee(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleSchoolFees(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleHospitalFees(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleLifeInsurance(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBirthday(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleIncomeTaxRefund(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleHolidayFund(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGrandOperaNight(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleStockSale(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleDoctorFees(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBankError(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleLoanMatures(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleElectedChairman(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleTakeWalkOnBoardwalk(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleTakeTripToReading(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleMakeGeneralRepairs(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGoToJail(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGoBackThreeSpaces(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGetOutOfJailFree(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBankPaysDividend(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToNearestRailroad(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToNearestUtility(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceStCharlesPlace(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToIllinois(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToGo(Board board, VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Infrastructure

        public void Start(int players)
        {
            if (players < GameConstants.MinimumPlayers || players > GameConstants.MaximumPlayers) throw new MonopolyException($"Invalid players count {players}");

            CurrentPlayers.Clear();
            Board.Clear();

            foreach (var player in this.players) player.Clear();

            var allTokens = TokenNames.AllTokens();
            var totalTokens = TokenNames.TotalTokens();
            var random = new Random();
            bool noRandom = false;

            for (int index = 0; CurrentPlayers.Count < players;)
            {
                if (!noRandom) index = random.Next(totalTokens);

                var player = this.players.First(x => x.Name == allTokens[index]);

                if (!CurrentPlayers.Contains(player))
                {
                    CurrentPlayers.Add(player);
                    if (noRandom) noRandom = false;
                }
                else
                {
                    if (++index == totalTokens) index = 0;
                    noRandom = true;
                }
            }

            Board.Init(CurrentPlayers);
            currentPlayerIndex = 0;
            currentPlayer = CurrentPlayers[currentPlayerIndex];

            Running = true;
        }

        public void ThrowDice()
        {
            if (!Running) return;

            Random random = new Random();
            firstDie = random.Next(GameConstants.MaximumDiceValue) + 1;
            secondDie = random.Next(GameConstants.MaximumDiceValue) + 1;
            var space = Board.Spaces.FirstOrDefault(x => x.Visitors.Contains(currentPlayer));

            if (space == null) throw new MonopolyException($"Player '{currentPlayer.Name}' wasn't found at board.");

            space.Visitors.Remove(currentPlayer);
            var spaceIndex = Board.Spaces.IndexOf(space);
            spaceIndex += firstDie + secondDie;

            if (spaceIndex >= Board.Spaces.Count()) spaceIndex %= Board.Spaces.Count();

            Board.Spaces[spaceIndex].Visitors.Add(currentPlayer);
            currentPlayerIndex = (currentPlayerIndex + 1) % CurrentPlayers.Count;
            currentPlayer = CurrentPlayers[currentPlayerIndex];

            DiceThrown?.Invoke(this, new Events.DiceThrownArgs { CurrentPlayer = currentPlayer, FirstDie = firstDie, SecondDie = secondDie });
        }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            foreach (var titleDeed in titleDeeds) titleDeed.Log(stream);
            foreach (var card in communityChest) card.Log(stream);
            foreach (var card in chance) card.Log(stream);
            foreach (var player in players) player.Log(stream);
            Board.Log(stream);
            stream.WriteLine($"Hotels: {hotels}");
            stream.WriteLine($"Houses: {houses}");
        }
#endif

        #endregion

        #region Properties

        public Board Board { get; }

        public List<VM.Player> CurrentPlayers { get; }

        public bool Running { get; private set; }

        #endregion
    }
}
