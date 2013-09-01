using System;
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
        private SettingsManager _settingsManager;
        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            this._settingsManager = new SettingsManager(Settings.Default);

            new LoginView(this._settingsManager).ShowDialog();

            this._settingsManager.Save();
        }

    }
}
