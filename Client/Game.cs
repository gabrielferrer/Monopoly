using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
        public delegate void CurrentPlayerChangedHandler(object sender, Events.CurrentPlayerChangedArgs args);

        public event DiceThrownHandler DiceThrown;
        public event CurrentPlayerChangedHandler CurrentPlayerChanged;

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

            Spaces = new ObservableCollection<VM.Spaces.Space>();

            Spaces.Add(new VM.Spaces.Go(GameConstants.Salary, new VM.Spaces.SpaceDto { Row = 10, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.SouthEast, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.MediterraneanAvenue), new VM.Spaces.SpaceDto { Row = 10, Column = 9, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.FirstGroup }));
            Spaces.Add(new VM.Spaces.CommunityChest(new VM.Spaces.SpaceDto { Row = 10, Column = 8, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.BalticAvenue), new VM.Spaces.SpaceDto { Row = 10, Column = 7, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.FirstGroup }));
            Spaces.Add(new VM.Spaces.IncommeTax(GameConstants.IncommeTaxPercentage, GameConstants.IncommeTax, new VM.Spaces.SpaceDto { Row = 10, Column = 6, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Railroad(titleDeeds.First(x => x.Name == PropertyNames.ReadingRailroad), new VM.Spaces.SpaceDto { Row = 10, Column = 5, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.OrientalAvenue), new VM.Spaces.SpaceDto { Row = 10, Column = 4, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.SecondGroup }));
            Spaces.Add(new VM.Spaces.Chance(new VM.Spaces.SpaceDto { Row = 10, Column = 3, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.VermontAvenue), new VM.Spaces.SpaceDto { Row = 10, Column = 2, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.SecondGroup }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.ConnecticutAvenue), new VM.Spaces.SpaceDto { Row = 10, Column = 1, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.SecondGroup }));
            Spaces.Add(new VM.Spaces.Jail(new VM.Spaces.SpaceDto { Row = 10, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.SouthWest, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.StCharlesPlace), new VM.Spaces.SpaceDto { Row = 9, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, 0), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.ThirdGroup }));
            Spaces.Add(new VM.Spaces.Utility(titleDeeds.First(x => x.Name == PropertyNames.ElectricCompany), new VM.Spaces.SpaceDto { Row = 8, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.StatesAvenue), new VM.Spaces.SpaceDto { Row = 7, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.ThirdGroup }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.VirginiaAvenue), new VM.Spaces.SpaceDto { Row = 6, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.ThirdGroup }));
            Spaces.Add(new VM.Spaces.Railroad(titleDeeds.First(x => x.Name == PropertyNames.PennsylvaniaRailroad), new VM.Spaces.SpaceDto { Row = 5, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.StJamesPlace), new VM.Spaces.SpaceDto { Row = 4, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.FourthGroup }));
            Spaces.Add(new VM.Spaces.CommunityChest(new VM.Spaces.SpaceDto { Row = 3, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.TennesseeAvenue), new VM.Spaces.SpaceDto { Row = 2, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.FourthGroup }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.NewYorkAvenue), new VM.Spaces.SpaceDto { Row = 1, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.FourthGroup }));
            Spaces.Add(new VM.Spaces.Parking(new VM.Spaces.SpaceDto { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.NorthWest, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.KentuckyAvenue), new VM.Spaces.SpaceDto { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.FifthGroup }));
            Spaces.Add(new VM.Spaces.Chance(new VM.Spaces.SpaceDto { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.IndianaAvenue), new VM.Spaces.SpaceDto { Row = 0, Column = 3, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.FifthGroup }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.IllinoisAvenue), new VM.Spaces.SpaceDto { Row = 0, Column = 4, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.FifthGroup }));
            Spaces.Add(new VM.Spaces.Railroad(titleDeeds.First(x => x.Name == PropertyNames.BnORailroad), new VM.Spaces.SpaceDto { Row = 0, Column = 5, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.AtlanticAvenue), new VM.Spaces.SpaceDto { Row = 0, Column = 6, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.SixthGroup }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.VentnorAvenue), new VM.Spaces.SpaceDto { Row = 0, Column = 7, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.SixthGroup }));
            Spaces.Add(new VM.Spaces.Utility(titleDeeds.First(x => x.Name == PropertyNames.WaterWorks), new VM.Spaces.SpaceDto { Row = 0, Column = 8, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.MarvinGardens), new VM.Spaces.SpaceDto { Row = 0, Column = 9, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.SixthGroup }));
            Spaces.Add(new VM.Spaces.GoToJail(new VM.Spaces.SpaceDto { Row = 0, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.NorthEast, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.PacificAvenue), new VM.Spaces.SpaceDto { Row = 1, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.SeventhGroup }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.NorthCarolinaAvenue), new VM.Spaces.SpaceDto { Row = 2, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.SeventhGroup }));
            Spaces.Add(new VM.Spaces.CommunityChest(new VM.Spaces.SpaceDto { Row = 3, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.PennsylvaniaAvenue), new VM.Spaces.SpaceDto { Row = 4, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.SeventhGroup }));
            Spaces.Add(new VM.Spaces.Railroad(titleDeeds.First(x => x.Name == PropertyNames.ShortLine), new VM.Spaces.SpaceDto { Row = 5, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Chance(new VM.Spaces.SpaceDto { Row = 6, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.ParkPlace), new VM.Spaces.SpaceDto { Row = 7, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.EighthGroup }));
            Spaces.Add(new VM.Spaces.LuxuryTax(GameConstants.LuxuryTax, new VM.Spaces.SpaceDto { Row = 8, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = null }));
            Spaces.Add(new VM.Spaces.Street(titleDeeds.First(x => x.Name == PropertyNames.Boardwalk), new VM.Spaces.SpaceDto { Row = 9, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, 0), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.EighthGroup }));

            hotels = GameConstants.TotalHotels;
            houses = GameConstants.TotalHouses;
        }

        #endregion

        #region Rules

        private void RuleInherit(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBeautyContest(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleStreetRepairs(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleConsultancyFee(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleSchoolFees(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleHospitalFees(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleLifeInsurance(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBirthday(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleIncomeTaxRefund(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleHolidayFund(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGrandOperaNight(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleStockSale(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleDoctorFees(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBankError(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleLoanMatures(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleElectedChairman(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleTakeWalkOnBoardwalk(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleTakeTripToReading(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleMakeGeneralRepairs(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGoToJail(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGoBackThreeSpaces(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleGetOutOfJailFree(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleBankPaysDividend(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToNearestRailroad(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToNearestUtility(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceStCharlesPlace(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToIllinois(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        private void RuleAdvanceToGo(VM.Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Infrastructure

        public void Start(int players)
        {
            if (players < GameConstants.MinimumPlayers || players > GameConstants.MaximumPlayers) throw new MonopolyException($"Invalid players count {players}");

            CurrentPlayers.Clear();
            foreach (var space in Spaces) space.Clear();

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

            Spaces[0].SetVisitors(CurrentPlayers);
            foreach (var player in this.players) player.Money = GameConstants.InitialMoney;
            SetCurrentPlayer(0);

            Running = true;
        }

        private void SetCurrentPlayer(int index)
        {
            currentPlayerIndex = index;
            currentPlayer = CurrentPlayers[currentPlayerIndex];
            CurrentPlayerChanged?.Invoke(this, new Events.CurrentPlayerChangedArgs { CurrentPlayer = currentPlayer });
        }

        public void ThrowDice()
        {
            if (!Running) return;

            Random random = new Random();
            firstDie = random.Next(GameConstants.MaximumDiceValue) + 1;
            secondDie = random.Next(GameConstants.MaximumDiceValue) + 1;
            var sourceSpace = Spaces.FirstOrDefault(x => x.Visitors.Contains(currentPlayer));

            if (sourceSpace == null) throw new MonopolyException($"Player '{currentPlayer.Name}' wasn't found at board.");

            sourceSpace.Visitors.Remove(currentPlayer);
            var spaceIndex = Spaces.IndexOf(sourceSpace);
            spaceIndex += firstDie + secondDie;

            if (spaceIndex >= Spaces.Count()) spaceIndex %= Spaces.Count();

            var destinationSpace = Spaces[spaceIndex];
            destinationSpace.Visitors.Add(currentPlayer);
            SetCurrentPlayer((currentPlayerIndex + 1) % CurrentPlayers.Count);

            DiceThrown?.Invoke(this, new Events.DiceThrownArgs { FirstDie = firstDie, SecondDie = secondDie });

            destinationSpace.Check(currentPlayer);
        }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            foreach (var titleDeed in titleDeeds) titleDeed.Log(stream);
            foreach (var card in communityChest) card.Log(stream);
            foreach (var card in chance) card.Log(stream);
            foreach (var player in players) player.Log(stream);
            stream.WriteLine($"Hotels: {hotels}");
            stream.WriteLine($"Houses: {houses}");
        }
#endif

        #endregion

        #region Properties

        public ObservableCollection<VM.Spaces.Space> Spaces { get; }

        public List<VM.Player> CurrentPlayers { get; }

        public bool Running { get; private set; }

        #endregion
    }
}
