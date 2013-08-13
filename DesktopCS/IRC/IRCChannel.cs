using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.UserControls;
using NetIRC;

namespace DesktopCS.IRC
{
    class IRCChannel
    {
        private readonly TabUserControl _tab;
        private readonly Channel _channel;

        public IRCChannel(TabUserControl tab, Channel channel)
        {
            _tab = tab;
            _channel = channel;

            _channel.OnMessage += _channel_OnMessage;
        }

        void _channel_OnMessage(Channel source, User user, string message)
        {
            //_tab.Add
        }
    }
}
