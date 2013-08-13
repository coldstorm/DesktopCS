using System;
using System.Windows.Media;
using DesktopCS.Commands;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.Properties;
using DesktopCS.Views;

namespace DesktopCS.ViewModels
{
    class LoginViewModel
    {
        private readonly SettingsManager _settings;
        private readonly LoginData _loginData;

        public LoginViewModel(SettingsManager settings)
        {
            _settings = settings;
            _loginData = _settings.GetLoginData();

            LoginCommand = new LoginCommand(this);
        }

        public LoginCommand LoginCommand { get; private set; }

        public LoginData LoginData
        {
            get { return _loginData; }
        }

        public bool CanLogin
        {
            get { return LoginData.IsValid; }
        }

        public void Login()
        {
            _settings.SetLoginData(LoginData);

            var main = new MainView(new IRCClient(LoginData));
            main.Show();
        }
    }
}
