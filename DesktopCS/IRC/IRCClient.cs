using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DesktopCS.Helpers;
using DesktopCS.IRC;
using DesktopCS.Models;
using NetIRC;
using Channel = NetIRC.Channel;

namespace DesktopCS
{
    public class IRCClient
    {
        private readonly Dispatcher _dispatcher;
        private readonly TabManager _tabManager;
        private readonly Client _irc;

        public IRCClient(TabManager tabManager, LoginData loginData)
        {
            _dispatcher = Application.Current.Dispatcher;
            _tabManager = tabManager;
            _irc = new Client();
            _irc.OnConnect += irc_OnConnect;
            _irc.OnChannelJoin += _irc_OnChannelJoin;

            var cc = CountryCodeHelper.GetCC();
            var user = new NetIRC.User(loginData.Username, IdentHelper.Generate(loginData.ColorBrush, cc));
            _irc.Connect("frogbox.es", 6667, false, user);
        }

        void _irc_OnChannelJoin(Client client, Channel channel)
        {
            _dispatcher.BeginInvoke(new Action(() =>
                {

                    new IRCChannel(_tabManager[channel.FullName()], channel);
                    
                }));
        }

        void irc_OnConnect(Client client)
        {
            
            _irc.JoinChannel("DesktopCS");
            
        }

    }
}
