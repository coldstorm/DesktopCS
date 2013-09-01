using System;
using System.Diagnostics;
using System.Windows.Media;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.Properties;

namespace DesktopCS.Services
{
    public class SettingsManager
    {
        private readonly Settings _settings;

        internal SettingsManager(Settings settings)
        {
            _settings = settings;

            if (!_settings.HasUpgrade)
            {
                _settings.Upgrade();
                _settings.HasUpgrade = true;
            }
        }

        public LoginData GetLoginData()
        {
            string username = null;
            string password = null;
            SolidColorBrush colorBrush = Brushes.White;

            try
            {
                username = _settings.Username;

                if (!String.IsNullOrWhiteSpace(_settings.Password))
                    password = _settings.Password.DecryptString().ToInsecureString();

                if (!String.IsNullOrWhiteSpace(_settings.Color))
                    colorBrush = (SolidColorBrush) new BrushConverter().ConvertFrom(_settings.Color);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load settings: " + ex);
            }

            return new LoginData(username, password, colorBrush);
        }

        public void SetLoginData(LoginData loginData)
        {
            _settings.Username = loginData.Username;
            _settings.Password = loginData.Password.ToSecureString().EncryptString();
            _settings.Color = loginData.ColorBrush.ToString();
        }

        public void Save()
        {
            _settings.Save();
        }
    }
}
