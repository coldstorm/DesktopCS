using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using DesktopCS.Controls;
using DesktopCS.Helpers;
using DesktopCS.ViewModels;
using DesktopCS.Views;

namespace DesktopCS.Tabs
{
    public class TabManager
    {
        private readonly ObservableCollection<CSTabItem> _serverTabs = new ObservableCollection<CSTabItem>();
        private readonly ObservableCollection<CSTabItem> _channelTabs = new ObservableCollection<CSTabItem>();

        public ObservableCollection<CSTabItem> Tabs { get; private set; }

        public TabManager()
        {
            Tabs = new CompositeCollection<CSTabItem>(_serverTabs, _channelTabs);
        }

        void channel_Close(object sender, System.EventArgs e)
        {
            var tab = (CSTabItem)sender;
            _channelTabs.Remove(tab);
        }

        public Tab AddServer(string tabName)
        {
            var channel = new ServerTab(tabName);
            _serverTabs.Add(channel.TabItem);

            return channel;
        }

        public Tab AddChannel(string tabName)
        {
            var channel = new Tab(tabName);
            channel.Close += channel_Close;
            _serverTabs.Add(channel.TabItem);

            return channel;
        }
    }
}
