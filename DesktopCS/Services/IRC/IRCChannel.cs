using System;
using System.Windows;
using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;
using NetIRC.Messages.Send;

namespace DesktopCS.Services.IRC
{
    public class IRCChannel
    {
        private readonly IRCClient _ircClient;
        private readonly ChannelTab _channelTab;
        private readonly Channel _channel;

        public IRCChannel(IRCClient ircClient, ChannelTab channelTab, Channel channel)
        {
            this._ircClient = ircClient;
            this._ircClient.Input += this._ircClient_Input;

            this._channelTab = channelTab;
            this._channelTab.AddChat(new SystemMessageLine("You joined the room."));

            this._channel = channel;
            this._channel.OnMessage += this._channel_OnMessage;
            this._channel.OnJoin += _channel_OnJoin;
            this._channel.OnLeave += _channel_OnLeave;
            this._channel.OnNames += _channel_OnNames;
        }

        void _channel_OnNames(Channel source, string[] users)
        {
            foreach (var user in _channel.Users.Values)
            {
                _channelTab.AddUser(user.ToUserItem());
            }
        }

        private void _ircClient_Input(object sender, string text)
        {
            if (_channelTab.IsSelected)
            {
                this._ircClient.Send(new ChannelPrivate(_channel, text));
            }
        }

        void _channel_OnLeave(Channel source, User user)
        {
            _channelTab.RemoveUser(user.ToUserItem());
        }

        void _channel_OnJoin(Channel source, User user)
        {
            _channelTab.AddUser(user.ToUserItem());
        }

        private void _channel_OnMessage(Channel source, User user, string message)
        {
            _channelTab.AddChat(user, u => new MessageLine(u, message));
        }
    }
}
