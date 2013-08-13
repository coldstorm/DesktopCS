using System.Collections.ObjectModel;
using System.Windows.Media;
using DesktopCS.Controls;
using DesktopCS.Models;

namespace DesktopCS.ViewModels
{
    class MainViewModel
    {
        private readonly TabManager _tabManager;
        private IRCClient _irc;

        public MainViewModel(SettingsManager settings, IRCClient irc)
        {
            _tabManager = new TabManager(Tabs);
        }

        private readonly ObservableCollection<CSTabItem> _tabs = new ObservableCollection<CSTabItem>();

        public ObservableCollection<CSTabItem> Tabs
        {
            get { return _tabs; }
        }

        public MainViewModel(SettingsManager settingsManager, LoginData loginData)
        {
            _tabManager = new TabManager(Tabs);
            _irc = new IRCClient(_tabManager, loginData);
        }
    }
}
