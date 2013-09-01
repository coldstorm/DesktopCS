using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.Tabs;
using NetIRC;

namespace DesktopCS.IRC
{
    public class IRCChannel
    {
        private readonly Tab _channelTab;
        private readonly Channel _channel;

        public IRCChannel(TabManager tabManager, Channel channel)
        {
            _channel = channel;
            _channelTab = tabManager.AddChannel(channel.FullName);

             var line = new SystemMessageLine("You joined the room.");
            _channelTab.AddChat(line, false);

            _channel.OnMessage += _channel_OnMessage;
        }

        void _channel_OnMessage(Channel source, User user, string message)
        {
            var u = new UserListItem(user.NickName, IdentHelper.Parse(user.UserName));
            var line = new MessageLine(u, message);
            _channelTab.AddChat(line);
        }
    }
}
