using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using DesktopCS.Controls;

namespace DesktopCS.Tabs
{
    public class TabManager : UIInvoker
    {
        private readonly ObservableCollection<CSTabItem> _serverTabs = new ObservableCollection<CSTabItem>();
        private readonly ObservableCollection<CSTabItem> _channelTabs = new ObservableCollection<CSTabItem>();
        private readonly ObservableCollection<CSTabItem> _userTabs = new ObservableCollection<CSTabItem>();
        private readonly Dictionary<string, Tab> _userTabDictionary = new Dictionary<string, Tab>();

        public CompositeCollection Tabs { get; private set; }

        public TabManager()
        {
            Tabs = new CompositeCollection();

            var serverContainer = new CollectionContainer { Collection = _serverTabs };
            var channelContainer = new CollectionContainer { Collection = _channelTabs };

            Tabs.Add(serverContainer);
            Tabs.Add(channelContainer);
        }

        void channel_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            _channelTabs.Remove(tab.TabItem);
        }

        void user_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            _userTabDictionary.Remove(tab.Header);
            _userTabs.Remove(tab.TabItem);
        }

        void user_HeaderChange(object sender, string oldValue)
        {
            var tab = (Tab)sender;
            _userTabDictionary.Remove(oldValue);
            _userTabDictionary.Add(tab.Header, tab);
        }

        public Tab AddServer(string tabName)
        {
            if (InvokeRequired)
            {
                Tab value = null;
                Invoke(() => value = AddServer(tabName));
                return value;
            }

            var server = new ServerTab(tabName);
            _serverTabs.Add(server.TabItem);

            return server;
        }

        public Tab AddChannel(string tabName)
        {
            if (InvokeRequired)
            {
                Tab value = null;
                Invoke(() => value = AddChannel(tabName));
                return value;
            }

            var channel = new Tab(tabName);
            channel.Close += channel_Close;
            _channelTabs.Add(channel.TabItem);

            return channel;
        }

        public Tab AddOrGetUser(string tabName)
        {
            if (InvokeRequired)
            {
                Tab value = null;
                Invoke(() => value = AddOrGetUser(tabName));
                return value;
            }

            if (_userTabDictionary.ContainsKey(tabName))
            {
                return _userTabDictionary[tabName];
            }

            var user = new Tab(tabName);
            user.Close += user_Close;
            user.HeaderChange += user_HeaderChange;
            _userTabDictionary.Add(tabName, user);
            _userTabs.Add(user.TabItem);

            return user;
        }
    }
} 
