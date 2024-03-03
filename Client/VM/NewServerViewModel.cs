using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Monopoly.VM
{
    public class NewServerViewModel : WindowViewModel
    {
        #region Fields

        private string name;
        private string address;
        private string port;

        #endregion

        #region Constructors

        public NewServerViewModel()
        {
            OKCommand = new RelayCommand(param => CanCreateServer, param => CreateServer());
        }

        #endregion

        #region Infrastructure

        private void CreateServer()
        {
            var server = new Server
            {
                Name = Name,
                Address = Address,
                Port = int.Parse(Port)
            };

            if (CheckServer.Invoke(server))
            {
                Window?.Close();
            }
            else
            {
                MessageBox.Show($"A server with name '{Name}' already exists.", "Information", MessageBoxButton.OK);
            }
        }

        #endregion

        #region Properties

        private bool CanCreateServer => !string.IsNullOrWhiteSpace(Name)
            && IPAddress.TryParse(Address, out _)
            && int.TryParse(Port, out _);

        public Func<Server, bool> CheckServer { get; set; }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (address == value) return;
                address = value;
                OnPropertyChanged();
            }
        }

        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                if (port == value) return;
                port = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand OKCommand { get; }

        #endregion
    }
}
