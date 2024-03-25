using Shared;
using System.Windows.Input;

namespace Monopoly.VM
{
    public class NewGameWindowViewModel : WindowViewModel
    {
        #region Events

        public delegate void NewGameHandler(object sender, Events.NewGameArgs args);

        public event NewGameHandler NewGame;

        #endregion

        #region Constructors

        public NewGameWindowViewModel()
        {
            AcceptCommand = new RelayCommand(param => CanAccept, param => Accept());
            CancelCommand = new RelayCommand(param => Cancel());
        }

        #endregion

        #region Infrastructure

        private void Accept()
        {
            Window?.Close();
            NewGame?.Invoke(this, new Events.NewGameArgs());
        }

        private void Cancel()
        {
            Window?.Close();
        }

        #endregion

        #region Properties

        private bool CanAccept => true;

        #endregion

        #region Commands

        public ICommand AcceptCommand { get; }

        public ICommand CancelCommand { get; }

        #endregion
    }
}
