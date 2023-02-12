using Monopoly.Events;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Monopoly.VM
{
    public class GameWindowViewModel : ViewModelBase
    {
        #region Fields

        private Game game;
        private double[] rowDefinitions;
        private double[] columnDefinitions;
        private string currentPlayerName;
        private int currentPlayerMoney;
        private Brush currentPlayerColor;
        private int firstDie;
        private int secondDie;

        #endregion

        #region Constructors

        public GameWindowViewModel()
        {
            NewGameCommand = new RelayCommand(param => CanStartNewGame, param => NewGameParameters());
            ExitCommand = new RelayCommand(param => CanExit(), param => Exit());
            ThrowDiceCommand = new RelayCommand(param => ThrowDice());

            rowDefinitions = new double[UI.Constants.BoardRows];
            columnDefinitions = new double[UI.Constants.BoardColumns];

            for (int row = 0; row < UI.Constants.BoardRows; row++) rowDefinitions[row] = double.NaN;
            for (int column = 0; column < UI.Constants.BoardColumns; column++) columnDefinitions[column] = double.NaN;

            game = new Game();
            game.DiceThrown += OnDiceThrown;
            game.CurrentPlayerChanged += OnCurrentPlayerChanged;
#if DEBUG
            //using (var stream = new System.IO.StreamWriter(@"C:\Users\Gabriel\Desktop\log.txt"))
            //{
            //game.Log(stream);
            //}
#endif
        }

        private void NewGameParameters()
        {
            var window = new UI.NewGameWindow();
            window.DataContext.NewGame += OnNewGame;
            window.Show();
        }

        private void OnNewGame(object sender, NewGameArgs args)
        {
            game.Start(args.Players);
        }

        private void OnDiceThrown(object sender, DiceThrownArgs args)
        {
            FirstDie = args.FirstDie;
            SecondDie = args.SecondDie;
        }

        private void OnCurrentPlayerChanged(object sender, CurrentPlayerChangedArgs args)
        {
            CurrentPlayerName = args.CurrentPlayer.Name;
            CurrentPlayerMoney = args.CurrentPlayer.Money;
            CurrentPlayerColor = args.CurrentPlayer.PlayerColor;
        }

        private bool CanExit()
        {
            if (game.Running)
            {
                var result = MessageBox.Show("There is a game in progress. Do you want to exit?", "Exit Game", MessageBoxButton.YesNo);
                return result == MessageBoxResult.Yes;
            }

            return true;
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void ThrowDice()
        {
            game.ThrowDice();
        }

        #endregion

        #region Properties

        private bool CanStartNewGame => !game.Running;

        public Brush WindowColor => new SolidColorBrush(UI.Constants.BoardColor);

        public ObservableCollection<double> RowDefinitions => new ObservableCollection<double>(rowDefinitions);

        public ObservableCollection<double> ColumnDefinitions => new ObservableCollection<double>(columnDefinitions);

        public int Rows => rowDefinitions.Length;

        public int Columns => columnDefinitions.Length;

        public ObservableCollection<Spaces.Space> Spaces => game.Spaces;

        public string CurrentPlayerName
        {
            get
            {
                return currentPlayerName;
            }
            set
            {
                if (currentPlayerName == value) return;
                currentPlayerName = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPlayerMoney
        {
            get
            {
                return currentPlayerMoney;
            }
            set
            {
                if (currentPlayerMoney == value) return;
                currentPlayerMoney = value;
                OnPropertyChanged();
            }
        }

        public Brush CurrentPlayerColor
        {
            get
            {
                return currentPlayerColor;
            }
            set
            {
                if (currentPlayerColor == value) return;
                currentPlayerColor = value;
                OnPropertyChanged();
            }
        }

        public int FirstDie
        {
            get
            {
                return firstDie;
            }
            set
            {
                if (firstDie == value) return;
                firstDie = value;
                OnPropertyChanged();
            }
        }

        public int SecondDie
        {
            get
            {
                return secondDie;
            }
            set
            {
                if (secondDie == value) return;
                secondDie = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand NewGameCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand ThrowDiceCommand { get; }

        #endregion
    }
}
