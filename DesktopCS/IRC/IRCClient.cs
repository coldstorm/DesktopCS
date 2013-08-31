using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DesktopCS.Helpers;
using DesktopCS.IRC;
using DesktopCS.Models;
using DesktopCS.Tabs;
using NetIRC;

namespace DesktopCS
{
    public class IRCClient
    {
        private readonly Dispatcher _dispatcher;
        private readonly TabManager _tabManager;
        private readonly Tab _serverTab;
        private readonly Client _irc;
        

        public IRCClient(TabManager tabManager, LoginData loginData)
        {
            _dispatcher = Application.Current.Dispatcher;
            _tabManager = tabManager;

            _serverTab = _tabManager.AddServer("Server");

            _irc = new Client();
            _irc.OnConnect += irc_OnConnect;
            _irc.OnChannelJoin += _irc_OnChannelJoin;
            _irc.OnNotice += _irc_OnNotice;

            var cc = CountryCodeHelper.GetCC();
            var user = new User(loginData.Username, IdentHelper.Generate(loginData.ColorBrush, cc));
            _irc.Connect("frogbox.es", 6667, false, user);
        }

        void _irc_OnNotice(Client client, User source, string notice)
        {
                _dispatcher.BeginInvoke(new Action(() =>
                    {

                        UserListItem u = null;
                        if (source != null)
                            u = new UserListItem(source.NickName, IdentHelper.Parse(source.UserName));
                        var line = new MessageLine(u, notice);
                        _serverTab.AddChat(line);

                    }));
            
        }

        void _irc_OnChannelJoin(Client client, Channel channel)
        {
            _dispatcher.BeginInvoke(new Action(() =>
                {

                    new IRCChannel(_tabManager.AddChannel(channel.FullName), channel);
                    
                }));
        }

        void irc_OnConnect(Client client)
        {
            _irc.JoinChannel("test");
        }
    }
}
