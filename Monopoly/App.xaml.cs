using System.Windows;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var gameWindowViewModel = new VM.GameWindowViewModel();
            var window = new UI.GameWindow();
            window.DataContext = gameWindowViewModel;
            Current.MainWindow = window;
            window.Show();
        }
    }
}
