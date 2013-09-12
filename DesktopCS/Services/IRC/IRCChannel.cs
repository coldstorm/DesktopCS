using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCChannel : IRCBase
    {
        private readonly IRCClient _ircClient;
        private readonly ChannelTab _channelTab;
        private readonly Channel _channel;

        public IRCChannel(IRCClient ircClient, ChannelTab channelTab, Channel channel) 
            : base(ircClient, channelTab)
        {
            this._ircClient = ircClient;
            this._ircClient.Input += this._ircClient_Input;
            this._ircClient.ChannelLeave += this._ircClient_ChannelLeave;

            this._channelTab = channelTab;
            this._channelTab.Close += this._channelTab_Close;
            this._channelTab.Header = channel.FullName; // Correct case
            this._channelTab.AddJoin();

            this._channel = channel;
            this._channel.OnMessage += this._channel_OnMessage;
            this._channel.OnJoin += this._channel_OnJoin;
            this._channel.OnLeave += this._channel_OnLeave;
            this._channel.OnNames += this._channel_OnNames;
            this._channel.OnTopicChange += this._channel_OnTopicChange;
        }

        private void AddUser(Channel channel, User user)
        {
            UserItem userItem = user.ToUserItem(this._channel);
            this._channelTab.AddUser(userItem);

            new IRCChannelUser(this._ircClient, this._channelTab, userItem, user, channel);
        }

        #region Event Handlers

        private void _channelTab_Close(object sender, EventArgs e)
        {
            this._ircClient.Send(this._channel.Part());
            this.Dispose();
        }

        private void _ircClient_Input(object sender, string text)
        {
            if (this._channelTab.IsSelected)
            {
                this._ircClient.Send(this._channel.SendMessage(text));
            }
        }

        private void _ircClient_ChannelLeave(object sender, Channel channel)
        {
            this.Run(() =>
            {
                if (channel == _channel)
                {
                    this._channelTab.Users.Clear();
                    this.Dispose();
                }
            });
        }

        private void _channel_OnNames(Channel source, string[] users)
        {
            this.Run(() =>
            {
                this._channelTab.Users.Clear();
                foreach (var user in this._channel.Users.Values)
                {
                    this.AddUser(this._channel, user);
                }
            });
        }

        private void _channel_OnLeave(Channel source, User user, string reason)
        {
            this.Run(() =>
            {
                this._channelTab.AddLeave(user.NickName, reason);
                this._channelTab.RemoveUser(user.ToUserItem(source));
            });
        }

        private void _channel_OnJoin(Channel source, User user)
        {
            this.Run(() =>
            {
                this._channelTab.AddJoin(user.NickName);
                this.AddUser(this._channel, user);
            });
        }

        private void _channel_OnMessage(Channel source, User user, string message)
        {
            this.Run(() => this._channelTab.AddChat(user, this._channel, message));
        }

        private void _channel_OnTopicChange(Channel source, ChannelTopic topic)
        {
            this._channelTab.Topic.Author = topic.Author.ToUserItem();
            this._channelTab.Topic.Content = topic.Message;
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();

            this._ircClient.Input -= this._ircClient_Input;
            this._ircClient.ChannelLeave -= this._ircClient_ChannelLeave;

            this._channelTab.Close -= this._channelTab_Close;

            this._channel.OnMessage -= this._channel_OnMessage;
            this._channel.OnJoin -= this._channel_OnJoin;
            this._channel.OnLeave -= this._channel_OnLeave;
            this._channel.OnNames -= this._channel_OnNames;
            this._channel.OnTopicChange -= this._channel_OnTopicChange;
        }

        #endregion
    }
}
