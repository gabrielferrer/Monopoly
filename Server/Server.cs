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
        private static ConcurrentBag<TcpClient> clients = new ConcurrentBag<TcpClient>();
        private Configuration configuration;
        private Thread connectionListener;

        #endregion

        #region Infrastructure

        private static bool Stop()
        {
            return Interlocked.Read(ref stopped) != 0;
        }

        private static void ConnectionListenerStart(object data)
        {
            var parameters = data as ConnectionListenerParameters;

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

                while (!Stop())
                {
                    var client = listener.AcceptTcpClient();
                    clients.Add(client);
                    //Console.WriteLine($"New client connected from {client.Client.RemoteEndPoint.}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Thread for client '{parameters.Address}:{parameters.Port}' failed. {e.Message}");
            }
        }

        private static void PrintWelcome()
        {
            var assembly = Assembly.GetCallingAssembly();     
            Console.WriteLine($"Monopoly server '{assembly.GetName().Name}', version {assembly.GetName().Version}");
        }

        private void ReadConfiguration()
        {
            configuration = new Configuration();
            configuration.Address = ConfigurationManager.AppSettings["address"];
            configuration.Port = int.Parse(ConfigurationManager.AppSettings["port"]);
        }

        private void Listen()
        {
            var parameters = new ConnectionListenerParameters
            {
                Address = configuration.Address,
                Port = configuration.Port
            };

            connectionListener = new Thread(ConnectionListenerStart);
            connectionListener.Start(parameters);
        }

        private void Initialize()
        {
            ReadConfiguration();
            Listen();
        }

        private void Run()
        {
            while (!Stop())
            {
                
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
