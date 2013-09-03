using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.MVVM;

namespace DesktopCS.Services
{
    public class TabManager : UIInvoker
    {
        private readonly ObservableCollection<CSTabItem> _serverTabs = new ObservableCollection<CSTabItem>();
        private readonly ObservableCollection<CSTabItem> _channelTabs = new ObservableCollection<CSTabItem>();
        private readonly ObservableCollection<CSTabItem> _userTabs = new ObservableCollection<CSTabItem>();
        private readonly Dictionary<string, Tab> _tabDictionary = new Dictionary<string, Tab>();

        public CompositeCollection<CSTabItem> Tabs { get; private set; }

        public TabManager()
        {
            this.Tabs = new CompositeCollection<CSTabItem>(this._serverTabs, this._channelTabs, this._userTabs);
        }

        void channel_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            this._tabDictionary.Remove(tab.Header);
            this._channelTabs.Remove(tab.TabItem);
        }

        void user_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            this._tabDictionary.Remove(tab.Header);
            this._userTabs.Remove(tab.TabItem);
        }

        void user_HeaderChange(object sender, string oldValue)
        {
            var tab = (Tab)sender;
            this._tabDictionary.Remove(oldValue);
            this._tabDictionary.Add(tab.Header, tab);
        }

        public Tab SelectedTab
        {
            get
            {
                return _tabDictionary.Values.FirstOrDefault(t => t.IsSelected);
            }
            set
            {
                value.TabItem.IsSelected = true;
            }
        }

        public ServerTab AddServer(string tabName)
        {
            if (this.InvokeRequired)
            {
                ServerTab value = null;
                this.Invoke(() => value = this.AddServer(tabName));
                return value;
            }

            if (this._tabDictionary.ContainsKey(tabName))
            {
                return (ServerTab)this._tabDictionary[tabName];
            }

            var server = new ServerTab(tabName);
            this._tabDictionary.Add(tabName, server);
            this._serverTabs.Add(server.TabItem);

            return server;
        }

        public ChannelTab AddChannel(string tabName)
        {
            if (this.InvokeRequired)
            {
                ChannelTab value = null;
                this.Invoke(() => value = this.AddChannel(tabName));
                return value;
            }

            if (this._tabDictionary.ContainsKey(tabName))
            {
                return (ChannelTab)this._tabDictionary[tabName];
            }

            var channel = new ChannelTab(tabName);
            channel.Close += this.channel_Close;
            this._tabDictionary.Add(tabName, channel);
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

            if (this._tabDictionary.ContainsKey(tabName))
            {
                return this._tabDictionary[tabName];
            }

            var user = new Tab(tabName);
            user.Close += this.user_Close;
            user.HeaderChange += this.user_HeaderChange;
            this._tabDictionary.Add(tabName, user);
            this._userTabs.Add(user.TabItem);

            return user;
        }
    }
} 
