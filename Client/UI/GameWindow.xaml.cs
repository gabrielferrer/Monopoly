using Monopoly.VM;
using System;
using System.ComponentModel;

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

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!DataContext.CanClose())
            {
                e.Cancel = true;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            DataContext.Close();
        }
    }
}
