using System;
using DesktopCS.Properties;
using DesktopCS.Views;

namespace DesktopCS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private SettingsManager _settingsManager;
        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            _settingsManager = new SettingsManager(Settings.Default);

            new LoginView(_settingsManager).ShowDialog();

            _settingsManager.Save();
        }


    }
}
