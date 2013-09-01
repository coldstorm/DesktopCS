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
            get { return _tabs; }
        }

        public MainViewModel(SettingsManager settingsManager, LoginData loginData)
        {
            _tabManager = new TabManager();
            _tabs = _tabManager.Tabs;
            _irc = new IRCClient(_tabManager, loginData);
        }

        public bool CanChat
        {
            get { return true; }
        }

        public void Chat(string text)
        {
            _irc.Chat(text);
        }
    }
}
