using System;
using DesktopCS.Helpers;
using DesktopCS.Helpers.Parsers;
using DesktopCS.Helpers.Extentions;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services.Command;
using DesktopCS.Services.IRC.Messages.Receive.Numerics;
using NetIRC;
using NetIRC.Messages;
using NetIRC.Messages.Send;
using NetIRCHelper = DesktopCS.Helpers.NetIRCHelper;

namespace DesktopCS.Services.IRC
{
    // Handles Connecting and reconnecting to the IRC network and managing other IRC classes
    public class IRCClient : UIInvoker
    {
        private const string PasswordService = "NickServ";
        private const int MaxInputLenght = 400;

        private readonly CommandExecutor _commandExecutor = new CommandExecutor();
        private readonly TabManager _tabManager;
        private readonly LoginData _loginData;
        private Client _client;
        private bool _joining;

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

        public IRCClient(TabManager tabManager, LoginData loginData)
        {
            this._tabManager = tabManager;
            this._loginData = loginData;

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
            var user = new User(this._loginData.Username, IdentHelper.Generate(this._loginData.Color, cc));
            this._client.Connect("kaslai.us", 6667, false, user);
        }

        public void Chat(string text)
        {
            if (text.StartsWith("/", StringComparison.Ordinal))
            {
                ISendMessage message = this._commandExecutor.Execute(this._client, this._tabManager.SelectedTab, text.Substring(1));
                this.Send(message);
            }
            else
            {
                Tab target = this._tabManager.SelectedTab;
                if (!(target is ServerTab))
                {
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
        }

        public void Send(ISendMessage message)
        {
            this._client.Send(message);
        }


        public void Query(string nick)
        {
            this._tabManager.AddUser(nick).IsSelected = true;
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
                if (!String.IsNullOrWhiteSpace(this._loginData.Password))
                {
                    var message = new UserPrivate(PasswordService, "IDENTIFY " + this._loginData.Password);
                    this._client.Send(message);
                }
            });
        }

        void client_OnDisconnect(Client client)
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
            this.Run(() => this.OnChannelLeave(channel));
        }


        void client_OnSend(Client client, SendMessageEventArgs e)
        {
            this.Run(() =>
            {
                var joinMessage = e.Message as Join;
                if (joinMessage != null)
                {
                    this._tabManager.AddChannel(joinMessage.ChannelName).IsSelected = true;
                    this._joining = true;
                }

                var userPrivateMessage = e.Message as UserPrivate;
                if (userPrivateMessage != null)
                {
                    this._tabManager.AddUser(userPrivateMessage.Nick).IsSelected = true;
                    this.OnInput(userPrivateMessage.Message);
                }

                var channelPrivateMessage = e.Message as ChannelPrivate;
                if (channelPrivateMessage != null)
                {
                    this._tabManager.AddChannel(channelPrivateMessage.ChannelName).IsSelected = true;
                    this.OnInput(channelPrivateMessage.Message);
                }
            });
        }

        #endregion
    }
}
