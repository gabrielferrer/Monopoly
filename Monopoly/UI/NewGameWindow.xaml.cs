using System.Windows;

namespace Monopoly.UI
{
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        public NewGameWindow()
        {
            InitializeComponent();
            DataContext.Window = this;
        }

        public new VM.NewGameWindowViewModel DataContext
        {
            get
            {
                return (VM.NewGameWindowViewModel)base.DataContext;
            }
            set
            {
                base.DataContext = value;
            }
        }
    }
}
