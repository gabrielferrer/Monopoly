using Monopoly.VM;

namespace Monopoly.UI
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : View<GameWindowViewModel>
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        protected override GameWindowViewModel CreateDataContext()
        {
            return new GameWindowViewModel();
        }
    }
}
