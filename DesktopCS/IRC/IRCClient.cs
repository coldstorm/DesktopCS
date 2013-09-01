using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.Tabs;
using NetIRC;

namespace DesktopCS.IRC
{
    // Handles Connecting and reconnecting to the IRC network and managing other IRC classes
    public class IRCClient
    {
        private readonly TabManager _tabManager;
        private readonly LoginData _loginData;
        private Client _client;

        public IRCClient(TabManager tabManager, LoginData loginData)
        {
            _tabManager = tabManager;
            _loginData = loginData;

            Connect();
        }

        void _client_OnConnect(Client client)
        {
            new IRCServer(_tabManager, _client);
        }

        void _client_OnChannelJoin(Client client, Channel channel)
        {
            new IRCChannel(_tabManager, channel);
        }

        public void Connect()
        {
            _client = new Client();
            _client.OnConnect += _client_OnConnect;
            _client.OnChannelJoin += _client_OnChannelJoin;

            var cc = CountryCodeHelper.GetCC();
            var user = new User(_loginData.Username, IdentHelper.Generate(_loginData.ColorBrush, cc));
            _client.Connect("kaslai.us", 6667, false, user);
        }

        public void Chat(string text)
        {
            throw new NotImplementedException();
        }
    }
}
