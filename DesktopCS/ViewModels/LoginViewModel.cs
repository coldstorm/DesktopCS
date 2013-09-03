using DesktopCS.Commands;
using DesktopCS.Models;
using DesktopCS.Services;
using DesktopCS.Views;

namespace DesktopCS.ViewModels
{
    class LoginViewModel
    {
        private readonly SettingsManager _settings;
        private readonly LoginData _loginData;

        public LoginViewModel(SettingsManager settings)
        {
            this._settings = settings;
            this._loginData = this._settings.GetLoginData();

            this.LoginCommand = new LoginCommand(this);
        }

        public LoginCommand LoginCommand { get; private set; }

        public LoginData LoginData
        {
            get { return this._loginData; }
        }

        public bool CanLogin
        {
            get { return this.LoginData.IsValid; }
        }

        public void Login()
        {
            this._settings.SetLoginData(this.LoginData);

            var main = new MainView(this._settings, this.LoginData);
            main.Show();
        }
    }
}
