using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;
using NetIRC.Messages.Send;

namespace DesktopCS.Services.IRC
{
    // Handles Connecting and reconnecting to the IRC network and managing other IRC classes
    public class IRCClient
    {
        private IRCServer _ircServer;
        private readonly TabManager _tabManager;
        private readonly LoginData _loginData;
        private Client _client;

        public IRCClient(TabManager tabManager, LoginData loginData)
        {
            this._tabManager = tabManager;
            this._loginData = loginData;

            this.Connect();
        }

        void _client_OnConnect(Client client)
        {
            _ircServer = new IRCServer(this._tabManager, client);

            _client.JoinChannel("test");
        }

        void _client_OnChannelJoin(Client client, Channel channel)
        {
            new IRCChannel(this._tabManager, channel);
        }

        public void Connect()
        {
            this._client = new Client();
            this._client.OnConnect += this._client_OnConnect;
            this._client.OnChannelJoin += this._client_OnChannelJoin;

            var cc = CountryCodeHelper.GetCC();
            var user = new User(this._loginData.Username, IdentHelper.Generate(this._loginData.Color, cc));
            this._client.Connect("kaslai.us", 6667, false, user);
        }

        public void Chat(string text)
        {
            _ircServer.Chat(text);
            _client.Send(new UserPrivate(_tabManager.SelectedTab.Header, text));
        }
    }
}
