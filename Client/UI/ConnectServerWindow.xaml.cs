using Monopoly.VM;

namespace Monopoly.UI
{
    /// <summary>
    /// Interaction logic for ConnectServerWindow.xaml
    /// </summary>
    public partial class ConnectServerWindow : View<ConnectServerViewModel>
    {
        public ConnectServerWindow()
        {
            InitializeComponent();
        }

        protected override ConnectServerViewModel CreateDataContext()
        {
            return new ConnectServerViewModel();
        }
    }
}
