using System.Collections.ObjectModel;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.Tabs;

namespace DesktopCS.ViewModels
{
    class MainViewModel
    {
        private readonly TabManager _tabManager;
        private IRCClient _irc;

        public MainViewModel(SettingsManager settings, IRCClient irc)
        {
            _tabManager = new TabManager();
            _tabs = _tabManager.Tabs;
        }

        private readonly ObservableCollection<CSTabItem> _tabs;

        public ObservableCollection<CSTabItem> Tabs
        {
            get { return _tabs; }
        }

        public MainViewModel(SettingsManager settingsManager, LoginData loginData)
        {
            _tabManager = new TabManager();
            _tabs = _tabManager.Tabs;
            _irc = new IRCClient(_tabManager, loginData);
        }
    }
}
