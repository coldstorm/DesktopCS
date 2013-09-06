using DesktopCS.Models;
using DesktopCS.Properties;
using DesktopCS.Services;
using DesktopCS.Views;

namespace DesktopCS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            // Show windows
            bool? showDialog = new LoginView().ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new MainView().ShowDialog();
            }

            // Save settings and shut down
            SettingsManager.Value.Save();
            this.Shutdown();
        }

    }
}
