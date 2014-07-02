using System;
using System.Diagnostics;
using System.Windows.Media;
using DesktopCS.Helpers;
using DesktopCS.Helpers.Extensions;
using DesktopCS.Models;
using DesktopCS.Properties;
using NetIRC.Messages.Receive;

namespace DesktopCS.Services
{
    public class SettingsManager
    {
        public static SettingsManager Value = new SettingsManager(Settings.Default);
        
        private readonly Settings _settings;

        private SettingsManager(Settings settings)
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
            string channels = null;
            Color color = ColorHelper.ChatColor;

            try
            {
                username = this._settings.Username;
                channels = this._settings.Channels;

                if (!String.IsNullOrWhiteSpace(this._settings.Password))
                    password = this._settings.Password.DecryptString().ToInsecureString();

                if (!String.IsNullOrWhiteSpace(this._settings.Color))
                    color = ColorHelper.FromString(this._settings.Color);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load settings: " + ex);
            }

            return new LoginData(username, password, channels, color);
        }

        public void SetLoginData(LoginData loginData)
        {
            this._settings.Username = loginData.Username;
            this._settings.Password = loginData.Password.ToSecureString().EncryptString();
            this._settings.Channels = loginData.Channels;
            this._settings.Color = loginData.Color.ToString();
        }

        public IRCSettings GetIRCSettings()
        {
            bool soundNotifications = this._settings.SoundNotifications;
            bool desktopNotifications = this._settings.DesktopNotifications;

            return new IRCSettings(soundNotifications, desktopNotifications);
        }

        public void SetIRCSettings(IRCSettings settings)
        {
            this._settings.SoundNotifications = settings.SoundNotifications;
            this._settings.DesktopNotifications = settings.DesktopNotifications;
            this.TriggerOnIRCSettingsChanged();
        }

        public WindowGeometry GetWindowGeometry()
        {
            double top = this._settings.WindowTop;
            double left = this._settings.WindowLeft;
            double height = this._settings.WindowHeight;
            double width = this._settings.WindowWidth;
            bool isMaximized = this._settings.WindowMaximized;

            return new WindowGeometry(top, left, height, width, isMaximized);
        }

        public void SetWindowGeometry(WindowGeometry window)
        {
            this._settings.WindowTop = window.Top;
            this._settings.WindowLeft = window.Left;
            this._settings.WindowHeight = window.Height;
            this._settings.WindowWidth = window.Width;
            this._settings.WindowMaximized = window.IsMaximized;
        }

        public void Save()
        {
            this._settings.Save();
        }

        public delegate void OnIRCSettingsChangedHandler(IRCSettings settings);
        public event OnIRCSettingsChangedHandler OnIRCSettingsChanged;

        internal void TriggerOnIRCSettingsChanged()
        {
            if (this.OnIRCSettingsChanged != null)
            {
                this.OnIRCSettingsChanged(GetIRCSettings());
            }
        }
    }
}
