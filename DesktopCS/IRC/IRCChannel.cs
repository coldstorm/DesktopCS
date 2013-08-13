using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.UserControls;
using Channel = NetIRC.Channel;
using User = NetIRC.User;

namespace DesktopCS.IRC
{
    class IRCChannel
    {
        private readonly Dispatcher _dispatcher;
        private readonly Models.Channel _tab;
        private readonly Channel _channel;

        public IRCChannel(Models.Channel tab, Channel channel)
        {
            _dispatcher = Application.Current.Dispatcher;
            _tab = tab;
            _channel = channel;

             var line = new SystemMessageLine("You joined the room.");
            _tab.AddChat(line);

            _channel.OnMessage += _channel_OnMessage;
        }

        void _channel_OnMessage(Channel source, User user, string message)
        {
            _dispatcher.BeginInvoke(new Action(() =>
                {
                    var u = new Models.User(UserRank.None, user.NickName, user.UserName, user.HostName,
                                            IdentHelper.Parse(user.UserName));
                    var line = new MessageLine(u, message);
                    _tab.AddChat(line);
                }));

        }
    }
}
