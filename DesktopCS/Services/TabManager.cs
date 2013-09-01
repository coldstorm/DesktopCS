using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using DesktopCS.Controls;
using DesktopCS.Models;

namespace DesktopCS.Services
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
            this.Tabs = new CompositeCollection();

            var serverContainer = new CollectionContainer { Collection = this._serverTabs };
            var channelContainer = new CollectionContainer { Collection = this._channelTabs };

            this.Tabs.Add(serverContainer);
            this.Tabs.Add(channelContainer);
        }

        void channel_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            this._channelTabs.Remove(tab.TabItem);
        }

        void user_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            this._userTabDictionary.Remove(tab.Header);
            this._userTabs.Remove(tab.TabItem);
        }

        void user_HeaderChange(object sender, string oldValue)
        {
            var tab = (Tab)sender;
            this._userTabDictionary.Remove(oldValue);
            this._userTabDictionary.Add(tab.Header, tab);
        }

        public Tab AddServer(string tabName)
        {
            if (this.InvokeRequired)
            {
                Tab value = null;
                this.Invoke(() => value = this.AddServer(tabName));
                return value;
            }

            var server = new ServerTab(tabName);
            this._serverTabs.Add(server.TabItem);

            return server;
        }

        public Tab AddChannel(string tabName)
        {
            if (this.InvokeRequired)
            {
                Tab value = null;
                this.Invoke(() => value = this.AddChannel(tabName));
                return value;
            }

            var channel = new Tab(tabName);
            channel.Close += this.channel_Close;
            this._channelTabs.Add(channel.TabItem);

            return channel;
        }

        public Tab AddOrGetUser(string tabName)
        {
            if (this.InvokeRequired)
            {
                Tab value = null;
                this.Invoke(() => value = this.AddOrGetUser(tabName));
                return value;
            }

            if (this._userTabDictionary.ContainsKey(tabName))
            {
                return this._userTabDictionary[tabName];
            }

            var user = new Tab(tabName);
            user.Close += this.user_Close;
            user.HeaderChange += this.user_HeaderChange;
            this._userTabDictionary.Add(tabName, user);
            this._userTabs.Add(user.TabItem);

            return user;
        }
    }
} 
