using Server.Models;
using Shared;
using Shared.Messages;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace Server
{
    class Server
    {
        #region Fields

        private static long stopped = 0;
        private static ConcurrentDictionary<ClientKey, Thread> clients = new ConcurrentDictionary<ClientKey, Thread>();
        private static ConcurrentBag<Game> games = new ConcurrentBag<Game>();
        private static MessageService messageService = new MessageService();
        private Configuration configuration;
        private Thread connectionListener;

        #endregion

        #region Infrastructure

        private static bool Stopped()
        {
            return Interlocked.Read(ref stopped) != 0;
        }

        private static void ConnectionListener(object data)
        {
            var parameters = data as ConnectionParameters;

            if (parameters == null)
            {
                Console.WriteLine($"Couldn't start client. Invalid data type '{nameof(data)}'");
                return;
            }

            try
            {
                var address = IPAddress.Parse(parameters.Address);
                var port = parameters.Port;
                var listener = new TcpListener(address, port);
                listener.Start();

                Console.WriteLine($"Listening on {address}:{port}");

                while (!Stopped())
                {
                    var client = listener.AcceptTcpClient();

                    if (!(client.Client.RemoteEndPoint is IPEndPoint iPEndPoint))
                    {
                        Console.WriteLine($"Client remote end point is not in the form IP address and port");
                        continue;
                    }

                    Console.WriteLine($"New client connected from {iPEndPoint.Address}:{iPEndPoint.Port}");

                    var clientThread = new Thread(ClientListener);
                    var clientKey = new ClientKey
                    {
                        Address = iPEndPoint.Address.ToString(),
                        Port = iPEndPoint.Port
                    };

                    clients.TryAdd(clientKey, clientThread);

                    var clientThreadParams = new ClientThreadParameters
                    {
                        Client = client,
                        ClientKey = clientKey
                    };

                    clientThread.Start(clientThreadParams);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Thread for client '{parameters.Address}:{parameters.Port}' failed. {e.Message}");
            }
        }

        private static void ClientListener(object data)
        {
            if (!(data is ClientThreadParameters clientThreadParameters))
            {
                Console.WriteLine($"Invalid client thread parameters: {nameof(data)}");
                return;
            }

            while (!Stopped())
            {
                var message = messageService.Read(clientThreadParameters.Client);

                if (message is CreateGameMessage createGameMessage)
                {
                    HandleCreateGameMessage(clientThreadParameters, createGameMessage);
                }
                else
                {
                    Console.WriteLine($"Unknown message type: {nameof(message)}");
                }
            }
        }

        private static void HandleCreateGameMessage(ClientThreadParameters clientThreadParameters, CreateGameMessage createGameMessage)
        {
            Game existingGame = null;
            var gamesEnumerator = games.GetEnumerator();

            while (gamesEnumerator.MoveNext())
            {
                var game = gamesEnumerator.Current;

                if (game.Players.ContainsKey(clientThreadParameters.ClientKey))
                {
                    messageService.Write(clientThreadParameters.Client, new PlayerInGameMessage());
                    existingGame = game;
                    break;
                }
            }

            if (existingGame == null)
            {
                var game = new Game();
                var clientKey = new ClientKey(clientThreadParameters.ClientKey);
                var player = new Player
                {
                    IsOwner = true
                };

                game.Players.Add(clientKey, player);

                games.Add(game);

                messageService.Write(clientThreadParameters.Client, new GameCreatedMessage());
            }
        }

        private void PrintWelcome()
        {
            var assembly = Assembly.GetCallingAssembly();     
            Console.WriteLine($"Monopoly server '{assembly.GetName().Name}', version {assembly.GetName().Version}");
        }

        private void RegisterMessageService()
        {
            messageService = new MessageService();
            ServiceLocator.Instance.Register<IMessageService>(messageService);
        }

        private void ReadConfiguration()
        {
            configuration = new Configuration();
            configuration.Address = ConfigurationManager.AppSettings["address"];
            configuration.Port = int.Parse(ConfigurationManager.AppSettings["port"]);
        }

        private void Listen()
        {
            var parameters = new ConnectionParameters
            {
                Address = configuration.Address,
                Port = configuration.Port
            };

            connectionListener = new Thread(ConnectionListener);
            connectionListener.Start(parameters);
        }

        private void Initialize()
        {
            RegisterMessageService();
            ReadConfiguration();
            Listen();
        }

        private void Run()
        {
            while (!Stopped())
            {
                // Game logic here.
            }
        }

        public void Start()
        {
            try
            {
                PrintWelcome();
                Initialize();
                Run();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Server error. {e.Message}");
            }
        }

        #endregion
    }
}
