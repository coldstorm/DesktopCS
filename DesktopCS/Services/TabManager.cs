using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DesktopCS.Controls;
using DesktopCS.Models;

namespace DesktopCS.Services
{
    public class TabManager
    {
        public ObservableCollection<CSTabItem> ServerTabs { get; private set; }
        public ObservableCollection<CSTabItem> ChannelTabs { get; private set; }
        public ObservableCollection<CSTabItem> UserTabs { get; private set; }
        private readonly Dictionary<string, Tab> _tabDictionary = new Dictionary<string, Tab>();

        public TabManager()
        {
            this.ServerTabs = new ObservableCollection<CSTabItem>();
            this.ChannelTabs = new ObservableCollection<CSTabItem>();
            this.UserTabs = new ObservableCollection<CSTabItem>();
        }

        void channel_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            this._tabDictionary.Remove(tab.Header);
            this.ChannelTabs.Remove(tab.TabItem);
        }

        void user_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            this._tabDictionary.Remove(tab.Header);
            this.UserTabs.Remove(tab.TabItem);
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
                return this._tabDictionary.Values.FirstOrDefault(t => t.IsSelected);
            }
            set
            {
                value.IsSelected = true;
            }
        }

        public ServerTab AddServer(string tabName)
        {
            if (this._tabDictionary.ContainsKey(tabName))
            {
                return (ServerTab)this._tabDictionary[tabName];
            }

            var server = new ServerTab(tabName);
            this._tabDictionary.Add(tabName, server);
            this.ServerTabs.Add(server.TabItem);

            return server;
        }

        public ChannelTab AddChannel(string tabName)
        {
            if (this._tabDictionary.ContainsKey(tabName))
            {
                return (ChannelTab)this._tabDictionary[tabName];
            }

            var channel = new ChannelTab(tabName);
            channel.Close += this.channel_Close;
            this._tabDictionary.Add(tabName, channel);
            this.ChannelTabs.Add(channel.TabItem);

            return channel;
        }

        public Tab AddOrGetUser(string tabName)
        {
            if (this._tabDictionary.ContainsKey(tabName))
            {
                return this._tabDictionary[tabName];
            }

            var user = new Tab(tabName);
            user.Close += this.user_Close;
            user.HeaderChange += this.user_HeaderChange;
            this._tabDictionary.Add(tabName, user);
            this.UserTabs.Add(user.TabItem);

            return user;
        }
    }
} 
