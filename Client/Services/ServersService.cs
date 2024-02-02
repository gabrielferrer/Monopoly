using Monopoly.VM;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace Monopoly.Services
{
    public class ServersService : IServersService
    {
        #region Constants

        private const string ServersFileName = "servers.data";
        private readonly object locker = new object();

        #endregion

        public IEnumerable<Server> LoadServers()
        {
            lock (locker)
            {
                if (!File.Exists(ServersFileName))
                {
                    return new Server[0];
                }

                using (var fileStream = new FileStream(ServersFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return Serializer.Deserialize<IEnumerable<Server>>(fileStream);
                }
            }
        }

        public void SaveServers(IEnumerable<Server> servers)
        {
            lock (locker)
            {
                using (var fileStream = new FileStream(ServersFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Serializer.Serialize(fileStream, servers);
                }
            }
        }
    }
}
