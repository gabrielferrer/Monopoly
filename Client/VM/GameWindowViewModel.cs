using Monopoly.Events;
using Shared;
using Shared.Messages;
using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Monopoly.VM
{
    public class GameWindowViewModel : WindowViewModel
    {
        #region Fields

        private TcpClient client;
        private Thread readThread;
        private Thread writeThread;
        private readonly object writeMessageLock = new object();
        private Message writeMessage;
        private bool connected;
        private bool playing;
        private double[] rowDefinitions;
        private double[] columnDefinitions;
        private string currentPlayerName;
        private int currentPlayerMoney;
        private Brush currentPlayerColor;
        private int firstDie;
        private int secondDie;
        private string status;

        #endregion

        #region Constructors

        public GameWindowViewModel()
        {
            ConnectCommand = new RelayCommand(param => CanConnect, param => Connect());
            DisconnectCommand = new RelayCommand(param => CanDisconnect, param => Disconnect());
            NewGameCommand = new RelayCommand(param => CanCreateNewGame, param => NewGame());
            JoinGameCommand = new RelayCommand(param => CanJoinGame, param => JoinGame());
            ThrowDiceCommand = new RelayCommand(param => ThrowDice());

            rowDefinitions = new double[UI.Constants.BoardRows];
            columnDefinitions = new double[UI.Constants.BoardColumns];

            for (int row = 0; row < UI.Constants.BoardRows; row++)
            {
                rowDefinitions[row] = double.NaN;
            }

            for (int column = 0; column < UI.Constants.BoardColumns; column++)
            {
                columnDefinitions[column] = double.NaN;
            }

            Spaces = new ObservableCollection<Spaces.Space>();

            Spaces.Add(new Spaces.Go(GameConstants.Salary, new Spaces.SpaceDto { Row = 10, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.SouthEast, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.MediterraneanAvenue, 60, 2, 250, new[] { 10, 30, 90, 160 }, 30, 50, 50), new Spaces.SpaceDto { Row = 10, Column = 9, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.FirstGroup }));
            Spaces.Add(new Spaces.CommunityChest(new Spaces.SpaceDto { Row = 10, Column = 8, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.BalticAvenue, 60, 4, 450, new[] { 20, 60, 180, 320 }, 30, 50, 50), new Spaces.SpaceDto { Row = 10, Column = 7, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.FirstGroup }));
            Spaces.Add(new Spaces.IncommeTax(GameConstants.IncommeTaxPercentage, GameConstants.IncommeTax, new Spaces.SpaceDto { Row = 10, Column = 6, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = null }));
            Spaces.Add(new Spaces.Railroad(new Titles.Railroad(PropertyNames.ReadingRailroad, 200, 25, new[] { 50, 100, 200 }, 100), new Spaces.SpaceDto { Row = 10, Column = 5, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.OrientalAvenue, 100, 6, 50, new[] { 30, 90, 270, 400 }, 550, 50, 50), new Spaces.SpaceDto { Row = 10, Column = 4, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.SecondGroup }));
            Spaces.Add(new Spaces.Chance(new Spaces.SpaceDto { Row = 10, Column = 3, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.VermontAvenue, 100, 6, 50, new[] { 30, 90, 270, 400 }, 550, 50, 50), new Spaces.SpaceDto { Row = 10, Column = 2, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.SecondGroup }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.ConnecticutAvenue, 120, 8, 60, new[] { 40, 100, 300, 450 }, 600, 50, 50), new Spaces.SpaceDto { Row = 10, Column = 1, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.South, StripeColor = UI.PropertyColors.SecondGroup }));
            Spaces.Add(new Spaces.Jail(new Spaces.SpaceDto { Row = 10, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.SouthWest, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.StCharlesPlace, 140, 10, 70, new[] { 50, 150, 450, 625 }, 750, 100, 100), new Spaces.SpaceDto { Row = 9, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, 0), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.ThirdGroup }));
            Spaces.Add(new Spaces.Utility(new Titles.Utility(PropertyNames.ElectricCompany, 150, 75), new Spaces.SpaceDto { Row = 8, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.StatesAvenue, 140, 10, 70, new[] { 50, 150, 450, 625 }, 750, 100, 100), new Spaces.SpaceDto { Row = 7, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.ThirdGroup }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.VirginiaAvenue, 160, 12, 80, new[] { 60, 180, 500, 700 }, 900, 100, 100), new Spaces.SpaceDto { Row = 6, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.ThirdGroup }));
            Spaces.Add(new Spaces.Railroad(new Titles.Railroad(PropertyNames.PennsylvaniaRailroad, 200, 25, new[] { 50, 100, 200 }, 100), new Spaces.SpaceDto { Row = 5, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.StJamesPlace, 180, 14, 90, new[] { 70, 200, 550, 750 }, 950, 100, 100), new Spaces.SpaceDto { Row = 4, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.FourthGroup }));
            Spaces.Add(new Spaces.CommunityChest(new Spaces.SpaceDto { Row = 3, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.TennesseeAvenue, 180, 14, 90, new[] { 70, 200, 550, 750 }, 950, 100, 100), new Spaces.SpaceDto { Row = 2, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.FourthGroup }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.NewYorkAvenue, 200, 16, 100, new[] { 80, 220, 600, 800 }, 1000, 100, 100), new Spaces.SpaceDto { Row = 1, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.West, StripeColor = UI.PropertyColors.FourthGroup }));
            Spaces.Add(new Spaces.Parking(new Spaces.SpaceDto { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.NorthWest, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.KentuckyAvenue, 220, 18, 110, new[] { 90, 250, 700, 875 }, 1050, 150, 150), new Spaces.SpaceDto { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.FifthGroup }));
            Spaces.Add(new Spaces.Chance(new Spaces.SpaceDto { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.IndianaAvenue, 220, 18, 110, new[] { 90, 250, 700, 875 }, 1050, 150, 150), new Spaces.SpaceDto { Row = 0, Column = 3, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.FifthGroup }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.IllinoisAvenue, 240, 20, 120, new[] { 100, 300, 750, 925 }, 1100, 150, 150), new Spaces.SpaceDto { Row = 0, Column = 4, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.FifthGroup }));
            Spaces.Add(new Spaces.Railroad(new Titles.Railroad(PropertyNames.BnORailroad, 200, 25, new[] { 50, 100, 200 }, 100), new Spaces.SpaceDto { Row = 0, Column = 5, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.AtlanticAvenue, 260, 22, 130, new[] { 110, 330, 800, 975 }, 1150, 150, 150), new Spaces.SpaceDto { Row = 0, Column = 6, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.SixthGroup }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.VentnorAvenue, 260, 22, 130, new[] { 110, 330, 800, 975 }, 1150, 150, 150), new Spaces.SpaceDto { Row = 0, Column = 7, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.SixthGroup }));
            Spaces.Add(new Spaces.Utility(new Titles.Utility(PropertyNames.WaterWorks, 150, 75), new Spaces.SpaceDto { Row = 0, Column = 8, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.MarvinGardens, 280, 24, 140, new[] { 120, 360, 850, 1025 }, 1200, 150, 150), new Spaces.SpaceDto { Row = 0, Column = 9, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(0, UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.North, StripeColor = UI.PropertyColors.SixthGroup }));
            Spaces.Add(new Spaces.GoToJail(new Spaces.SpaceDto { Row = 0, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.NorthEast, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.PacificAvenue, 300, 26, 150, new[] { 130, 390, 900, 1100 }, 1275, 200, 200), new Spaces.SpaceDto { Row = 1, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.SeventhGroup }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.NorthCarolinaAvenue, 300, 26, 150, new[] { 130, 390, 900, 1100 }, 1275, 200, 200), new Spaces.SpaceDto { Row = 2, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.SeventhGroup }));
            Spaces.Add(new Spaces.CommunityChest(new Spaces.SpaceDto { Row = 3, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.PennsylvaniaAvenue, 320, 28, 160, new[] { 150, 450, 1000, 1200 }, 1400, 200, 200), new Spaces.SpaceDto { Row = 4, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.SeventhGroup }));
            Spaces.Add(new Spaces.Railroad(new Titles.Railroad(PropertyNames.ShortLine, 200, 25, new[] { 50, 100, 200 }, 100), new Spaces.SpaceDto { Row = 5, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = null }));
            Spaces.Add(new Spaces.Chance(new Spaces.SpaceDto { Row = 6, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.ParkPlace, 350, 35, 175, new[] { 175, 500, 1100, 1300 }, 1500, 200, 200), new Spaces.SpaceDto { Row = 7, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.EighthGroup }));
            Spaces.Add(new Spaces.LuxuryTax(GameConstants.LuxuryTax, new Spaces.SpaceDto { Row = 8, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness), Orientation = UI.BoardCellOrientation.East, StripeColor = null }));
            Spaces.Add(new Spaces.Street(new Titles.Street(PropertyNames.Boardwalk, 400, 50, 200, new[] { 200, 600, 1400, 1700 }, 2000, 200, 200), new Spaces.SpaceDto { Row = 9, Column = 10, RowSpan = 1, ColumnSpan = 1, Border = new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, 0), Orientation = UI.BoardCellOrientation.East, StripeColor = UI.PropertyColors.EighthGroup }));

            client = new TcpClient();
            //game = new Game();
            //game.DiceThrown += OnDiceThrown;
            //game.CurrentPlayerChanged += OnCurrentPlayerChanged;
#if DEBUG
            //using (var stream = new System.IO.StreamWriter(@"C:\Users\Gabriel\Desktop\log.txt"))
            //{
            //game.Log(stream);
            //}
#endif
        }

        private void Connect()
        {
            var window = new UI.ConnectServerWindow();

            window.DataContext.ConnectServer = server =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        if (!client.Connected)
                        {
                            client.Connect(server.Address, server.Port);
                            Application.Current.Dispatcher.Invoke(() => Status = $"Connected to server at {server.Address}:{server.Port}.");
                        }

                        Application.Current.Dispatcher.Invoke(() => IsConnected = true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"There was an error connecting to server at {server.Address}:{server.Port}. {e.Message}");
                        Application.Current.Dispatcher.Invoke(() => Status = "There was an error connecting to the server.");
                        return;
                    }

                    readThread?.Abort();
                    writeThread?.Abort();

                    readThread = new Thread(ReadThread);
                    writeThread = new Thread(WriteThread);

                    readThread.Start(client);
                    writeThread.Start(client);
                });
            };

            window.Show();
        }

        private void Disconnect()
        {
            Task.Run(() =>
            {
                try
                {
                    if (client.Connected)
                    {
                        client.Close();
                        Application.Current.Dispatcher.Invoke(() => Status = "Disconnected from the server.");
                    }

                    Application.Current.Dispatcher.Invoke(() => IsConnected = false);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"There was an error disconnecting from the server. {e.Message}");
                    Application.Current.Dispatcher.Invoke(() => Status = "There was an error disconnecting from the server.");
                }
            });
        }

        private void NewGame()
        {
            var window = new UI.NewGameWindow();

            window.DataContext.NewGame += OnNewGame;

            window.Show();
        }

        private void JoinGame()
        {
            //var window = new UI.JoinGameWindow();

            //window.DataContext.GameJoined += OnGameJoined;

            //window.Show();
        }

        private void ReadThread(object data)
        {
            var client = data as TcpClient;

            if (client == null)
            {
                Console.WriteLine($"Invalid data type '{nameof(data)}'");
                return;
            }

            var messageService = ServiceLocator.Instance.GetService<IMessageService>();

            while (true)
            {
                try
                {
                    var message = messageService.Read(client);
                    Application.Current.Dispatcher.Invoke(() => HandleMessage(message));
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine($"Finalizing {nameof(ReadThread)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception at {nameof(ReadThread)}. {e.Message}");
                    Application.Current.Dispatcher.Invoke(() => IsConnected = client?.Connected ?? false);
                    return;
                }
            }
        }

        private void WriteThread(object data)
        {
            var client = data as TcpClient;

            if (client == null)
            {
                Console.WriteLine($"Invalid data type '{nameof(data)}'");
                return;
            }

            var messageService = ServiceLocator.Instance.GetService<IMessageService>();

            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException)
                {
                    messageService.Write(client, GetMessageToWrite());
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine($"Finalizing {nameof(WriteThread)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception at {nameof(WriteThread)}. {e.Message}");
                    Application.Current.Dispatcher.Invoke(() => IsConnected = client?.Connected ?? false);
                    return;
                }
            }
        }

        private void HandleMessage(Message message)
        {
            if (message is GameCreatedMessage gameCreatedMessage)
            {
                HandleGameCreatedMessage(gameCreatedMessage);
            }

            if (message is PlayerInGameMessage playerInGameMessage)
            {
                HandlePlayerInGameMessage(playerInGameMessage);
            }
        }

        private void HandlePlayerInGameMessage(PlayerInGameMessage playerInGameMessage)
        {
            Status = $"You are already playing.";
        }

        private void HandleGameCreatedMessage(GameCreatedMessage gameCreatedMessage)
        {
            IsPlaying = true;
            Status = "Game started.";
        }

        private Message GetMessageToWrite()
        {
            Message message;

            lock (writeMessageLock)
            {
                message = writeMessage;
            }

            return message;
        }

        private void WriteMessage(Message message)
        {
            lock (writeMessageLock)
            {
                writeMessage = message.Clone();
            }

            writeThread?.Interrupt();
        }

        private void OnNewGame(object sender, NewGameArgs args)
        {
            WriteMessage(new CreateGameMessage());
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

        private void ThrowDice()
        {
            //game.ThrowDice();
        }

        public bool CanClose()
        {
            if (IsPlaying)
            {
                var result = MessageBox.Show("There is a game in progress. Do you want to exit?", "Exit Game", MessageBoxButton.YesNo);
                return result == MessageBoxResult.Yes;
            }

            return true;
        }

        public void Close()
        {
            if (client.Connected)
            {
                client.Close();
            }

            readThread?.Abort();
            writeThread?.Abort();
        }

        #endregion

        #region Properties

        public bool CanConnect => !IsConnected;

        public bool CanDisconnect => IsConnected;

        public bool CanCreateNewGame => IsConnected && !IsPlaying;

        public bool CanJoinGame => IsConnected && !IsPlaying;

        public bool IsConnected
        {
            get
            {
                return connected;
            }
            set
            {
                if (connected == value) return;
                connected = value;                
                OnPropertyChanged(nameof(CanCreateNewGame));
                OnPropertyChanged();
            }
        }

        public bool IsPlaying
        {
            get
            {
                return playing;
            }
            set
            {
                if (playing == value) return;
                playing = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<double> RowDefinitions => new ObservableCollection<double>(rowDefinitions);

        public ObservableCollection<double> ColumnDefinitions => new ObservableCollection<double>(columnDefinitions);

        public int Rows => rowDefinitions.Length;

        public int Columns => columnDefinitions.Length;

        public ObservableCollection<Spaces.Space> Spaces { get; }

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

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status == value) return;
                status = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand ConnectCommand { get; }

        public ICommand DisconnectCommand { get; }

        public ICommand NewGameCommand { get; }

        public ICommand JoinGameCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand ThrowDiceCommand { get; }

        #endregion
    }
}
