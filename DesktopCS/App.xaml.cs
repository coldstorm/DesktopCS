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
            // Load settings
            var settingsManager = new SettingsManager(Settings.Default);
            LoginData loginData = settingsManager.GetLoginData();

            // Show windows
            bool? showDialog = new LoginView(loginData).ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                settingsManager.SetLoginData(loginData);

                new MainView(loginData).ShowDialog();
            }

            // Shut down
            settingsManager.Save();
            this.Shutdown();
        }

    }
}
