using System;
using System.Windows;
using System.Windows.Threading;
using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.IRC
{
    class IRCChannel
    {
        private readonly Dispatcher _dispatcher;
        private readonly Tab _tab;
        private readonly Channel _channel;

        public IRCChannel(Tab tab, Channel channel)
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
                    var u = new UserListItem(user.NickName, IdentHelper.Parse(user.UserName));
                    var line = new MessageLine(u, message);
                    _tab.AddChat(line);
                }));

        }
    }
}
