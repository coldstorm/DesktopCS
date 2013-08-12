using System;
using System.Windows.Media;
using DesktopCS.Commands;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.Properties;

namespace DesktopCS.ViewModels
{
    class LoginViewModel
    {
        private readonly LoginData _loginData;

        public LoginViewModel()
        {
            string username = null;
            string password = null;
            SolidColorBrush colorBrush = Brushes.White;

            try
            {
                username = Settings.Default.Username;
                password = Settings.Default.Password.DecryptString().ToInsecureString();

                if (!String.IsNullOrWhiteSpace(Settings.Default.Color))
                    colorBrush = (SolidColorBrush) new BrushConverter().ConvertFrom(Settings.Default.Color);
                
            }
            catch
            {
                //TODO: Log
            }

            _loginData = new LoginData(username, password, colorBrush);


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
            Settings.Default.Username = LoginData.Username;
            Settings.Default.Password = LoginData.Password.ToSecureString().EncryptString();
            Settings.Default.Color = LoginData.ColorBrush.ToString();
            Settings.Default.Save();
        }
    }
}
