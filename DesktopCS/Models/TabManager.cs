using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.UserControls;

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

                var tab = new CSTabItem { Header = tabName };
                var tabContent = new TabUserControl(tab);
                tab.Content = tabContent;
                var channel = new Channel(tabContent, tab);
                
                _tabs.Add(tab);
                _channels.Add(tabName, channel);

                return channel;
            }
        }
    }
}
