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
            this._settings = settings;

            if (!this._settings.HasUpgrade)
            {
                this._settings.Upgrade();
                this._settings.HasUpgrade = true;
            }
        }

        public LoginData GetLoginData()
        {
            string username = null;
            string password = null;
            Color color = ColorHelper.ChatColor;

            try
            {
                username = this._settings.Username;

                if (!String.IsNullOrWhiteSpace(this._settings.Password))
                    password = this._settings.Password.DecryptString().ToInsecureString();

                if (!String.IsNullOrWhiteSpace(this._settings.Color))
                    color = ColorHelper.FromString(this._settings.Color);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load settings: " + ex);
            }

            return new LoginData(username, password, color);
        }

        public void SetLoginData(LoginData loginData)
        {
            this._settings.Username = loginData.Username;
            this._settings.Password = loginData.Password.ToSecureString().EncryptString();
            this._settings.Color = loginData.Color.ToString();
        }

        public void Save()
        {
            this._settings.Save();
        }
    }
}
