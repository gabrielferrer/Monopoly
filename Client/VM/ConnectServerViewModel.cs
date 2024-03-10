using Monopoly.Services;
using Shared;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Monopoly.VM
{
    public class ConnectServerViewModel : WindowViewModel
    {
        #region Fields

        private Server selectedServer;

        #endregion

        #region Constructors

        public ConnectServerViewModel()
        {
            NewCommand = new RelayCommand(param => New());
            ConnectCommand = new RelayCommand(param => CanConnect, param => Connect());

            var serversService = ServiceLocator.Instance.GetService<IServersService>();
            var servers = serversService.LoadServers();

            Servers = new ObservableCollection<Server>(servers);
        }

        #endregion

        #region Infrastructure

        private void SaveServers(ObservableCollection<Server> servers)
        {
            var serversService = ServiceLocator.Instance.GetService<IServersService>();
            serversService.SaveServers(servers);
        }

        private void New()
        {
            var window = new UI.NewServerWindow();

            window.DataContext.CheckServer = server =>
            {
                if (Servers.Contains(server))
                {
                    return false;
                }

                Servers.Add(server);

                SaveServers(Servers);

                return true;
            };

            window.Show();
        }

        private void Connect()
        {
            if (SelectedServer == null)
            {
                return;
            }

            ConnectServer(SelectedServer);

            Window?.Close();
        }

        #endregion

        #region Properties

        public Action<Server> ConnectServer { get; set; }

        private bool CanConnect => SelectedServer != null;

        public ObservableCollection<Server> Servers { get; }

        public Server SelectedServer
        {
            get
            {
                return selectedServer;
            }
            set
            {
                if (selectedServer == value) return;
                selectedServer = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand NewCommand { get; }

        public ICommand ConnectCommand { get; }

        #endregion
    }
}
