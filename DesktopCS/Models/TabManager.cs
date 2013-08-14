using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DesktopCS.Controls;
using DesktopCS.UserControls;
using DesktopCS.ViewModels;

namespace DesktopCS.Models
{
    public class TabManager
    {
        private readonly ObservableCollection<CSTabItem> _tabs;
        private readonly Dictionary<string, Tab> _channels = new Dictionary<string, Tab>(); 

        public TabManager(ObservableCollection<CSTabItem> tabs)
        {
            _tabs = tabs;
        }

        public Tab this[string tabName]
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
                var channel = new Tab(tabContentViewModel, tab);

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
