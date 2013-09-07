using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.ViewModels;
using DesktopCS.Views;

namespace DesktopCS.Services
{
    public class TabManager
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

            var server = (ServerTab)this.CreateTab((f, t) => new ServerTab(tabName, f, t));
            this._tabDictionary.Add(tabName, server);
            this._serverTabs.Add(server.TabItem);

            return server;
        }

        public ChannelTab AddChannel(string tabName)
        {
            if (this._tabDictionary.ContainsKey(tabName))
            {
                return (ChannelTab)this._tabDictionary[tabName];
            }

            var channel = (ChannelTab)this.CreateTab((f, t) => new ChannelTab(tabName, f, t));
            channel.Close += this.channel_Close;
            this._tabDictionary.Add(tabName, channel);
            this._channelTabs.Add(channel.TabItem);

            return channel;
        }

        public Tab AddUser(string tabName)
        {
            if (this._tabDictionary.ContainsKey(tabName))
            {
                return this._tabDictionary[tabName];
            }

            var user = this.CreateTab((f, t) => new Tab(tabName, f, t));
            user.Close += this.user_Close;
            user.HeaderChange += this.user_HeaderChange;
            this._tabDictionary.Add(tabName, user);
            this._userTabs.Add(user.TabItem);

            return user;
        }

        private Tab CreateTab(Func<FlowDocument, CSTabItem, Tab> callback)
        {
            var vm = new ChatTabContentViewModel();
            var tabView = new ChatTabContentView(vm);
            var tabItem = new CSTabItem { Content = tabView };
            return callback(vm.FlowDocument, tabItem);
        }
    }
} 
