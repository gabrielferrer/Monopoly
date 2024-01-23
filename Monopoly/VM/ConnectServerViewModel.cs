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
            Servers = new ObservableCollection<Server>();
            NewCommand = new RelayCommand(param => New());
            ConnectCommand = new RelayCommand(param => CanConnect, param => Connect());
        }

        #endregion

        #region Infrastructure

        private void New()
        {
        }

        private void Connect()
        {
            if (SelectedServer == null)
            {
                return;
            }

            Window?.Close();
        }

        #endregion

        #region Properties

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
