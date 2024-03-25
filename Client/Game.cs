using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Monopoly
{
    public class Game
    {
        #region Fields

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
            if (players < GameConstants.MinimumPlayers || players > GameConstants.MaximumPlayers)
            {
                throw new MonopolyException($"Invalid players count {players}");
            }

            CurrentPlayers.Clear();

            foreach (var space in Spaces)
            {
                space.Clear();
            }

            var allTokens = TokenNames.AllTokens();
            var totalTokens = TokenNames.TotalTokens();
            var random = new Random();
            bool noRandom = false;

            for (int index = 0; CurrentPlayers.Count < players;)
            {
                if (!noRandom)
                {
                    index = random.Next(totalTokens);
                }

                var player = this.players.First(x => x.Name == allTokens[index]);

                if (!CurrentPlayers.Contains(player))
                {
                    CurrentPlayers.Add(player);

                    if (noRandom)
                    {
                        noRandom = false;
                    }
                }
                else
                {
                    if (++index == totalTokens)
                    {
                        index = 0;
                    }

                    noRandom = true;
                }
            }

            Spaces[0].SetVisitors(CurrentPlayers);

            foreach (var player in this.players)
            {
                player.Money = GameConstants.InitialMoney;
            }

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
            if (!Running)
            {
                return;
            }

            Random random = new Random();
            firstDie = random.Next(GameConstants.MaximumDiceValue) + 1;
            secondDie = random.Next(GameConstants.MaximumDiceValue) + 1;
            var sourceSpace = Spaces.FirstOrDefault(x => x.Visitors.Contains(currentPlayer));

            if (sourceSpace == null)
            {
                throw new MonopolyException($"Player '{currentPlayer.Name}' wasn't found at board.");
            }

            sourceSpace.Visitors.Remove(currentPlayer);
            var spaceIndex = Spaces.IndexOf(sourceSpace);
            spaceIndex += firstDie + secondDie;

            if (spaceIndex >= Spaces.Count())
            {
                spaceIndex %= Spaces.Count();
            }

            var destinationSpace = Spaces[spaceIndex];
            destinationSpace.Visitors.Add(currentPlayer);
            SetCurrentPlayer((currentPlayerIndex + 1) % CurrentPlayers.Count);

            DiceThrown?.Invoke(this, new Events.DiceThrownArgs { FirstDie = firstDie, SecondDie = secondDie });

            destinationSpace.Check(currentPlayer);
        }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
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
