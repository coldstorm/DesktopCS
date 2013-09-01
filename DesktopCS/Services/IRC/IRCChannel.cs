using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCChannel
    {
        private readonly Tab _channelTab;
        private readonly Channel _channel;

        public IRCChannel(TabManager tabManager, Channel channel)
        {
            this._channel = channel;
            this._channelTab = tabManager.AddChannel(channel.FullName);

             var line = new SystemMessageLine("You joined the room.");
            this._channelTab.AddChat(line, false);

            this._channel.OnMessage += this._channel_OnMessage;
        }

        void _channel_OnMessage(Channel source, User user, string message)
        {
            var u = new UserListItem(user.NickName, IdentHelper.Parse(user.UserName));
            var line = new MessageLine(u, message);
            this._channelTab.AddChat(line);
        }
    }
}
