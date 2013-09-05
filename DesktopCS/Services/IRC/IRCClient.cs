using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services.IRC.Messages.Receive.Numerics;
using DesktopCS.Services.IRC.Messages.Send;
using NetIRC;
using NetIRC.Messages;

namespace DesktopCS.Services.IRC
{
    // Handles Connecting and reconnecting to the IRC network and managing other IRC classes
    public class IRCClient : UIInvoker, IDisposable
    {
        private readonly TabManager _tabManager;
        private readonly LoginData _loginData;
        private Client _client;

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

        public static event TextEventHandler ReceiveText;

        public static void OnReceiveText(object sender, string text)
        {
            TextEventHandler eventHandler = ReceiveText;
            if (eventHandler != null) eventHandler(sender, text);
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

        public IRCClient(TabManager tabManager, LoginData loginData)
        {
            ReceiveText += this.IRCClient_ReceiveText;

            this._tabManager = tabManager;
            this._loginData = loginData;

            this.Connect();
        }

        public void Connect()
        {
            this._client = new Client();
            this._client.RegisterMessage(typeof(Message));
            this._client.RegisterMessage(typeof(ParamMessage));

            this._client.OnConnect += this._client_OnConnect;
            this._client.OnChannelJoin += this._client_OnChannelJoin;
            this._client.OnChannelLeave += _client_OnChannelLeave;

            ServerTab tab = this._tabManager.AddServer("Server");
            new IRCServer(this, tab, this._client);

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

        private void _client_OnConnect(Client client)
        {
            //Run(() =>
            //{
            //   _client.JoinChannel("test");
            //});
        }

        private void _client_OnChannelJoin(Client client, Channel channel)
        {
            Run(() =>
            {
                ChannelTab tab = this._tabManager.AddChannel(channel.FullName);
                new IRCChannel(this, tab, channel);
            });
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
