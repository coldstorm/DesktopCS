using System.Collections.ObjectModel;
using System.Windows.Data;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.Services;
using DesktopCS.Services.IRC;

namespace DesktopCS.ViewModels
{
    class MainViewModel
    {
        private readonly TabManager _tabManager;
        private readonly IRCClient _irc;
        
        private readonly CompositeCollection _tabs;

        public CompositeCollection Tabs
        {
            get { return this._tabs; }
        }

        public MainViewModel(SettingsManager settingsManager, LoginData loginData)
        {
            this._tabManager = new TabManager();
            this._tabs = this._tabManager.Tabs;
            this._irc = new IRCClient(this._tabManager, loginData);
        }

        public bool CanChat
        {
            get { return true; }
        }

        public void Chat(string text)
        {
            this._irc.Chat(text);
        }
    }
}
