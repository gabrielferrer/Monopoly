using Monopoly.VM;

namespace Monopoly.UI
{
    /// <summary>
    /// Interaction logic for NewServerWindow.xaml
    /// </summary>
    public partial class NewServerWindow : View<NewServerViewModel>
    {
        public NewServerWindow()
        {
            InitializeComponent();
        }

        protected override NewServerViewModel CreateDataContext()
        {
            return new NewServerViewModel();
        }
    }
}
