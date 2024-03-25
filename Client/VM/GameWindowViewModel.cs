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
        private Game game;
        private bool connected;
        private bool gameInProcess;
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
            NewGameCommand = new RelayCommand(param => CanCreateNewGame, param => NewGame());
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

            client = new TcpClient();
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

        private void NewGame()
        {
            var window = new UI.NewGameWindow();

            window.DataContext.NewGame += OnNewGame;

            window.Show();
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
            GameInProcess = true;
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
            game.ThrowDice();
        }

        public bool CanClose()
        {
            if (game.Running)
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

        public bool CanCreateNewGame => IsConnected && !game.Running;

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

        public bool GameInProcess
        {
            get
            {
                return gameInProcess;
            }
            set
            {
                if (gameInProcess == value) return;
                gameInProcess = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand NewGameCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand ThrowDiceCommand { get; }

        #endregion
    }
}
