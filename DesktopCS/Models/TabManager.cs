using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.UserControls;
using DesktopCS.ViewModels;

namespace DesktopCS
{
    public class TabManager
    {
        private readonly ObservableCollection<CSTabItem> _tabs;
        private readonly Dictionary<string, Channel> _channels = new Dictionary<string, Channel>(); 

        public TabManager(ObservableCollection<CSTabItem> tabs)
        {
            _tabs = tabs;
        }

        public Channel this[string tabName]
        {
            get
            {
                if (_channels.ContainsKey(tabName))
                {
                    return _channels[tabName];
                }
                
                var tabContentViewModel = new TabUserControlViewModel();
                var tabContent = new TabUserControl(tabContentViewModel);
                var tab = new CSTabItem { Header = tabName, Content = tabContent };
                var channel = new Channel(tabContentViewModel, tab);

                tab.CloseTab += tab_CloseTab;
                _tabs.Add(tab);
                _channels.Add(tabName, channel);

                return channel;
            }
        }

        void tab_CloseTab(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            var tab = (CSTabItem) sender;
            _tabs.Remove(tab);

            if (_channels.ContainsKey((string) tab.Header))
            {
                //TODO: Disconnect from channel
            }

        }

        
    }
}
