using Monopoly.VM;

namespace Monopoly.UI
{
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : View<NewGameWindowViewModel>
    {
        public NewGameWindow()
        {
            InitializeComponent();
        }

        protected override NewGameWindowViewModel CreateDataContext()
        {
            return new NewGameWindowViewModel();
        }
    }
}
