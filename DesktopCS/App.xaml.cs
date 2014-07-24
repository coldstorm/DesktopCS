using DesktopCS.Services;
using DesktopCS.Views;
using DesktopCS.Helpers.Extensions;

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

        private void Application_Activated(object sender, System.EventArgs e)
        {
            // Stop flashing window when it receives focus
            WindowExtensions.StopFlashingWindow(App.Current.MainWindow);
        }
    }
}
