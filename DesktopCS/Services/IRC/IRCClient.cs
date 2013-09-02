using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;
using NetIRC.Messages;
using NetIRC.Messages.Send;

namespace DesktopCS.Services.IRC
{
    // Handles Connecting and reconnecting to the IRC network and managing other IRC classes
    public class IRCClient
    {
        private readonly TabManager _tabManager;
        private readonly LoginData _loginData;
        private Client _client;

        public delegate void InputHandler (object sender, string text);
        public event InputHandler Input;

        public Tab SelectedTab
        {
            get
            {
                return _tabManager.SelectedTab;
            }
            set
            {
                _tabManager.SelectedTab = value;
            }
        }

        protected virtual void OnInput(string text)
        {
            InputHandler handler = this.Input;
            if (handler != null) handler(this, text);
        }

        public IRCClient(TabManager tabManager, LoginData loginData)
        {
            this._tabManager = tabManager;
            this._loginData = loginData;

            this.Connect();
        }

        void _client_OnConnect(Client client)
        {

            _client.JoinChannel("test");
        }

        void _client_OnChannelJoin(Client client, Channel channel)
        {
            ChannelTab tab = _tabManager.AddChannel(channel.FullName);
            new IRCChannel(this, tab, channel);
        }

        public void Connect()
        {
            this._client = new Client();
            this._client.OnConnect += this._client_OnConnect;
            this._client.OnChannelJoin += this._client_OnChannelJoin;

            ServerTab tab = _tabManager.AddServer("Server");
            new IRCServer(this, tab, _client);

            var cc = CountryCodeHelper.GetCC();
            var user = new User(this._loginData.Username, IdentHelper.Generate(this._loginData.Color, cc));
            this._client.Connect("kaslai.us", 6667, false, user);
        }

        public void Chat(string text)
        {
            this.OnInput(text);
        }

        public void Send(ISendMessage message)
        {
            _client.Send(message);
        }
    }
}
