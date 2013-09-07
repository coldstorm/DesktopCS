using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services.IRC.Messages.Receive.Numerics;
using DesktopCS.Services.IRC.Messages.Send;
using NetIRC;
using NetIRC.Messages;
using NetIRC.Messages.Send;

namespace DesktopCS.Services.IRC
{
    // Handles Connecting and reconnecting to the IRC network and managing other IRC classes
    public class IRCClient : UIInvoker, IDisposable
    {
        private const string PasswordService = "NickServ";

        private readonly TabManager _tabManager;
        private readonly LoginData _loginData;
        private Client _client;

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
            ReceiveText += this.IRCClient_ReceiveText;

            this._tabManager = tabManager;
            this._loginData = loginData;

            this.Connect();

            AddUser(new User("Proco"));
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
            client.RegisterMessage(typeof(ParamMessage));

            client.OnChannelJoin += this._client_OnChannelJoin;
            client.OnChannelLeave += _client_OnChannelLeave;
            client.OnMessage += _client_OnMessage;
            client.OnConnect += client_OnConnect;

            this.AddServer(client);

            return client;
        }

        public void Connect()
        {
            this._client = CreateClient();

            var cc = CountryCodeHelper.GetCC();
            var user = new User(this._loginData.Username, IdentHelper.Generate(this._loginData.Color, cc));
            this._client.Connect("kaslai.us", 6667, false, user);
        }

        public void Chat(string text)
        {
            if (text.StartsWith("/", StringComparison.Ordinal))
            {
                this.Send(new Raw(text.Substring(1)));
            }
            else
            {
                this.OnInput(text);
            }
        }

        public void Send(ISendMessage message)
        {
            this._client.Send(message);
        }

        #region Events

        public delegate void TextEventHandler(object sender, string text);

        public event TextEventHandler Input;

        protected virtual void OnInput(string text)
        {
            TextEventHandler handler = this.Input;
            if (handler != null) handler(this, text);
        }

        public event TextEventHandler Text;

        protected virtual void OnText(string text)
        {
            TextEventHandler eventHandler = this.Text;
            if (eventHandler != null) eventHandler(this, text);
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

        public static event TextEventHandler ReceiveText;

        public static void OnReceiveText(object sender, string text)
        {
            TextEventHandler eventHandler = ReceiveText;
            if (eventHandler != null) eventHandler(sender, text);
        }

        #endregion

        #region Event Handlers

        private void IRCClient_ReceiveText(object sender, string text)
        {
            Run(() =>
            {
                if (sender == this._client)
                {
                    this.OnText(text);
                }
            });
        }

        private void client_OnConnect(Client client)
        {
            Run(() =>
            {
                var message = new UserPrivate(PasswordService, "IDENTIFY " + this._loginData.Password);
                this._client.Send(message);
            });
        }

        private void _client_OnMessage(Client client, User source, string message)
        {
            Run(() =>
            {
                this.AddUser(source);
                this.OnMessage(source, message);
            });
        }

        private void _client_OnChannelJoin(Client client, Channel channel)
        {
            Run(() => this.AddChannel(channel));
        }

        private void _client_OnChannelLeave(Client client, Channel channel)
        {
            Run(() => this.OnChannelLeave(channel));
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            ReceiveText -= this.IRCClient_ReceiveText;
        }

        #endregion
    }
}
