using Monopoly.VM;
using System.Collections.Generic;

namespace Monopoly.Services
{
    public interface IServersService
    {
        IEnumerable<Server> LoadServers();

        void SaveServers(IEnumerable<Server> servers);
    }
}
