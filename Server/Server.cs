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
        private static ConcurrentBag<Thread> clients = new ConcurrentBag<Thread>();
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

                    if (client.Client.RemoteEndPoint is IPEndPoint iPEndPoint)
                    {
                        Console.WriteLine($"New client connected from {iPEndPoint.Address}:{iPEndPoint.Port}");
                    }

                    var clientThread = new Thread(ClientListener);

                    clients.Add(clientThread);

                    clientThread.Start(client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Thread for client '{parameters.Address}:{parameters.Port}' failed. {e.Message}");
            }
        }

        private static void ClientListener(object data)
        {
            if (!(data is TcpClient client))
            {
                return;
            }

            while (!Stopped())
            {
                var message = messageService.Read(client);
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
