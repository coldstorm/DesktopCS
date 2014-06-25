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
            this._tabDictionary.Remove(tab.Header.ToLower());
            this.ChannelTabs.Remove(tab.TabItem);
        }

        void user_Close(object sender, EventArgs e)
        {
            var tab = (Tab)sender;
            this._tabDictionary.Remove(tab.Header.ToLower());
            this.UserTabs.Remove(tab.TabItem);
        }

        void tab_HeaderChange(object sender, string oldValue)
        {
            var tab = (Tab)sender;
            this._tabDictionary.Remove(oldValue.ToLower());
            this._tabDictionary.Add(tab.Header.ToLower(), tab);
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
            if (this._tabDictionary.ContainsKey(tabName.ToLower()))
            {
                return (ServerTab)this._tabDictionary[tabName.ToLower()];
            }

            var server = (ServerTab)this.CreateTab((f, t) => new ServerTab(tabName, f, t));
            this._tabDictionary.Add(tabName.ToLower(), server);
            this.ServerTabs.Add(server.TabItem);
            server.TabItem.IsConnected = true;

            return server;
        }

        public ChannelTab AddChannel(string tabName)
        {
            if (this._tabDictionary.ContainsKey(tabName.ToLower()))
            {
                return (ChannelTab)this._tabDictionary[tabName.ToLower()];
            }

            var channel = (ChannelTab)this.CreateTab((f, t) => new ChannelTab(tabName, f, t));
            channel.Close += this.channel_Close;
            channel.HeaderChange += this.tab_HeaderChange;
            this._tabDictionary.Add(tabName.ToLower(), channel);
            this.ChannelTabs.Add(channel.TabItem);

            return channel;
        }

        public Tab AddUser(string tabName)
        {
            if (this._tabDictionary.ContainsKey(tabName.ToLower()))
            {
                return this._tabDictionary[tabName.ToLower()];
            }

            var user = this.CreateTab((f, t) => new Tab(tabName, f, t));
            user.Close += this.user_Close;
            user.HeaderChange += this.tab_HeaderChange;
            this._tabDictionary.Add(tabName.ToLower(), user);
            this.UserTabs.Add(user.TabItem);
            user.TabItem.IsConnected = true;

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
