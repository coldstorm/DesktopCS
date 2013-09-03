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
            this._channel.OnJoin += this._channel_OnJoin;
            this._channel.OnLeave += this._channel_OnLeave;
            this._channel.OnNames += this._channel_OnNames;
        }

        private void AddUser(Channel channel, User user)
        {
            UserItem userItem = user.ToUserItem();
            this._channelTab.AddUser(userItem);
            new IRCChannelUser(userItem, user, channel);
        }

        void _channel_OnNames(Channel source, string[] users)
        {
            foreach (var user in this._channel.Users.Values)
            {
                this.AddUser(this._channel, user);
            }
        }

        private void _ircClient_Input(object sender, string text)
        {
            if (this._channelTab.IsSelected)
            {
                this._ircClient.Send(new ChannelPrivate(this._channel, text));
            }
        }

        void _channel_OnLeave(Channel source, User user)
        {
            this._channelTab.RemoveUser(user.ToUserItem());
        }

        void _channel_OnJoin(Channel source, User user)
        {
            this.AddUser(this._channel, user);
        }

        private void _channel_OnMessage(Channel source, User user, string message)
        {
            this._channelTab.AddChat(user, u => new MessageLine(u, message));
        }
    }
}
