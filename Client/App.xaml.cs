using Monopoly.Services;
using Shared;
using Shared.Messages;
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

            ServiceLocator.Instance.Register<IServersService>(new ServersService());
            ServiceLocator.Instance.Register<IMessageService>(new MessageService());

            var window = new UI.GameWindow();
            Current.MainWindow = window;
            window.Show();
        }
    }
}
