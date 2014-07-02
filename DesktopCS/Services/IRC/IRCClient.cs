using System;
using DesktopCS.Helpers;
using DesktopCS.Helpers.Extensions;
using DesktopCS.Helpers.Parsers;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services.Command;
using DesktopCS.Services.IRC.Messages.Receive.Numerics;
using NetIRC;
using NetIRC.IRCv3;
using NetIRC.Messages;
using NetIRC.Messages.Send;
using NetIRC.Messages.Send.IRCv3;
using NetIRC.Messages.Send.CTCP;
using System.Windows;
using System.Collections.Generic;

namespace DesktopCS.Services.IRC
{
    // Handles connecting and reconnecting to the IRC network and managing other IRC classes
    public class IRCClient : UIInvoker
    {
        private const string PasswordService = "NickServ";
        private const int MaxInputLenght = 400;

        private readonly CommandExecutor _commandExecutor = new CommandExecutor();
        private readonly TabManager _tabManager;
        private readonly LoginData _loginData;
        private IRCSettings _ircSettings;
        private Client _client;
        private bool _joining;

        public IRCSettings IRCSettings
        {
            get { return this._ircSettings; }
            set
            {
                this._ircSettings = value;
            }
        }

        public User User
        {
            get
            {
                return this._client.User;
            }
        }

        public bool SoundNotifications
        {
            get
            {
                return this._ircSettings.SoundNotifications;
            }
        }

        public bool DesktopNotifications
        {
            get
            {
                return this._ircSettings.DesktopNotifications;
            }
        }

        public Tab SelectedTab
        {
            get
            {
                return this._tabManager.SelectedTab;
            }
            set
            {
                this._tabManager.SelectedTab = value;
            }
        }

        public IRCClient(TabManager tabManager, LoginData loginData, IRCSettings ircSettings)
        {
            this._tabManager = tabManager;
            this._loginData = loginData;
            this._ircSettings = ircSettings;

            this.Connect();
        }

        private void AddServer(Client client)
        {
            const string tabName = "Server";

            // If this tab does not have a handler yet
            if (!this.DoPing(tabName))
            {
                ServerTab tab = this._tabManager.AddServer(tabName);
                new IRCServer(this, tab, client);
            }
        }

        private void AddChannel(Channel channel)
        {
            var tabName = channel.FullName;

            // If this tab does not have a handler yet
            if (!this.DoPing(tabName))
            {
                ChannelTab tab = this._tabManager.AddChannel(tabName);
                new IRCChannel(this, tab, channel);
                tab.TabItem.IsConnected = true;
                updateChannelList();
                tab.Close += (object sender, EventArgs e) => updateChannelList(); //Update saved channel list on exit
            }
        }

        private void AddUser(User user)
        {
            var tabName = user.NickName;

            // If this tab does not have a handler yet
            if (!this.DoPing(tabName))
            {
                Tab tab = this._tabManager.AddUser(tabName);
                new IRCUser(this, tab, user);
            }
        }

        private bool DoPing(string message)
        {
            var args = new PingEventArgs(message);
            this.OnPing(args);
            return args.Handled;
        }

        private Client CreateClient()
        {
            var client = new Client();
            client.RegisterMessage(typeof(Message));

            client.OnChannelJoin += this._client_OnChannelJoin;
            client.OnChannelLeave += this._client_OnChannelLeave;
            client.OnMessage += this._client_OnMessage;
            client.OnAction += this._client_OnAction;
            client.OnVersion += this._client_OnVersion;
            client.OnConnect += this.client_OnConnect;
            client.OnSend += this.client_OnSend;
            client.OnDisconnect += this.client_OnDisconnect;

            this.AddServer(client);

            return client;
        }

        public void Connect()
        {
            this._client = this.CreateClient();

            var cc = CountryCodeHelper.GetCC();
            var user = new User(this._loginData.Username, IdentHelper.Generate(this._loginData.Color, cc), "Desktop Coldstormer");
            this._client.Connect("irc.frogbox.es", 6667, false, user);
        }

        public void Chat(string text)
        {
            if (text.StartsWith("/", StringComparison.Ordinal))
            {
                this._commandExecutor.Execute(this._client, this._tabManager.SelectedTab, text.Substring(1));
            }

            else
            {
                this.OnInput(text);

                Tab target = this._tabManager.SelectedTab;
                if (target is ServerTab) return;

                foreach (var part in text.SplitByLength(MaxInputLenght))
                {
                    ISendMessage message;
                    if (NetIRCHelper.IsChannel(target.Header))
                        message = new ChannelPrivate(target.Header, part);
                    else
                        message = new UserPrivate(target.Header, part);

                    this.Send(message);
                }
            }
        }

        public void Send(ISendMessage message)
        {
            this._client.Send(message);
        }
        
        public void Query(string nick)
        {
            this._tabManager.AddUser(nick).IsSelected = true;
        }

        public void Kick(string nick)
        {
            this._client.Send(new Kick(this.SelectedTab.Header, nick));
        }

        public void BanNickName(string nick)
        {
            this._client.Send(new ChannelMode(this.SelectedTab.Header, "+b", nick + "!*@*"));
        }

        public void BanUserName(string user)
        {
            this._client.Send(new ChannelMode(this.SelectedTab.Header, "+b", "*!" + user + "@*"));
        }

        public void BanHostName(string host)
        {
            this._client.Send(new ChannelMode(this.SelectedTab.Header, "+b", "*!*@" + host));
        }

        #region Events

        public delegate void TextEventHandler(object sender, string text);

        public event TextEventHandler Input;

        protected virtual void OnInput(string text)
        {
            TextEventHandler handler = this.Input;
            if (handler != null) handler(this, text);
        }

        public delegate void ChannelLeaveEventHandler(object sender, Channel channel);

        public event ChannelLeaveEventHandler ChannelLeave;

        protected virtual void OnChannelLeave(Channel channel)
        {
            ChannelLeaveEventHandler handler = this.ChannelLeave;
            if (handler != null) handler(this, channel);
        }

        public event EventHandler<PingEventArgs> Ping;

        protected virtual void OnPing(PingEventArgs e)
        {
            EventHandler<PingEventArgs> handler = this.Ping;
            if (handler != null) handler(this, e);
        }

        public delegate void MessageHandler(object sender, User user, string message);

        public event MessageHandler Message;

        protected virtual void OnMessage(User user, string message)
        {
            MessageHandler handler = this.Message;
            if (handler != null) handler(this, user, message);
        }

        public event MessageHandler Action;

        protected virtual void OnAction(User user, string message)
        {
            MessageHandler handler = this.Action;
            if (handler != null) handler(this, user, message);
        }

        public delegate void VersionHandler(object sender, User user);

        public event VersionHandler Version;

        protected virtual void OnVersion(User user)
        {
            VersionHandler handler = this.Version;
            if (handler != null) handler(this, user);
        }

        public static event TextEventHandler ReceiveText;

        public static void OnReceiveText(object sender, string text)
        {
            TextEventHandler eventHandler = ReceiveText;
            if (eventHandler != null) eventHandler(sender, text);
        }

        #endregion

        #region Event Handlers

        private void client_OnConnect(Client client)
        {
            this.Run(() =>
            {
                client.User.OnIsAwayChange += User_OnIsAwayChange;

                // Enable Capabilities
                this._client.Send(new CapabilityRequest(Capability.MultiPrefix));
                this._client.Send(new CapabilityRequest(Capability.AwayNotify));
                this._client.Send(new CapabilityEnd());

                // NickServ
                if (!String.IsNullOrWhiteSpace(this._loginData.Password))
                {
                    var message = new UserPrivate(PasswordService, "IDENTIFY " + this._loginData.Password);
                    this._client.Send(message);
                }

                // Default channels
#if DEBUG
                this.Send(new Join("#test"));
#else
                //Split channel names and join them all
                string[] channelNames = this._loginData.Channels.Split(',');

                for (var i = 0; i < channelNames.Length; i++)
                {
                    this.Send(new Join(channelNames[i]));
                }
#endif
            });
        }

        private void client_OnDisconnect(Client client)
        {
            this.Run(this.Connect);
        }

        private void _client_OnMessage(Client client, User source, string message)
        {
            this.Run(() =>
            {
                this.AddUser(source);
                this.OnMessage(source, message);
            });
        }

        private void _client_OnAction(Client client, User source, string action)
        {
            this.Run(() =>
            {
                this.AddUser(source);
                this.OnAction(source, action);
            });
        }

        private void _client_OnVersion(Client client, User source)
        {
            this.Run(() =>
            {
                string versionString = String.Format("Coldstorm Desktop Client ({0})", VersionHelper.GetPublishedVersion());
                this.Send(new VersionReply(source, versionString));
                this.OnVersion(source);
            });
        }

        private void _client_OnChannelJoin(Client client, Channel channel)
        {
            this.Run(() =>
            {
                if (this._joining)
                {
                    this._joining = false;
                    this._tabManager.AddChannel(channel.FullName).IsSelected = true;
                }
                this.AddChannel(channel);
            });
        }

        private void _client_OnChannelLeave(Client client, Channel channel)
        {
            this.Run(() =>
            {
                ChannelTab tab = this._tabManager.AddChannel(channel.FullName);
                this.OnChannelLeave(channel);
                tab.TabItem.IsConnected = false;
            });
        }

        private void client_OnSend(Client client, SendMessageEventArgs e)
        {
            this.Run(() =>
            {
                var joinMessage = e.Message as Join;
                if (joinMessage != null)
                {
                    this._tabManager.AddChannel(joinMessage.ChannelName).IsSelected = true;
                    this._joining = true;
                }
            });
        }

        private void User_OnIsAwayChange(User user)
        {
            this.Send(new Whois(user));
        }

        private void updateChannelList()
        {
            //Update the channel list to save when logging out
            LoginData loginData = SettingsManager.Value.GetLoginData();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int i = 0;
            foreach (KeyValuePair<string, Channel> kv in _client.Channels)
            {
                sb.Append(kv.Key);
                if (i < _client.Channels.Count - 1) //Don't add comma to last channel
                    sb.Append(',');
                i++;
            }
            loginData.Channels = sb.ToString();
            SettingsManager.Value.SetLoginData(loginData);
        }
        #endregion
    }
}
