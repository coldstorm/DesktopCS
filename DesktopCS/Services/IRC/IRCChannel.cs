using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.MVVM;
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
            this._ircClient.ChannelLeave += _ircClient_ChannelLeave;

            this._channelTab = channelTab;
            this._channelTab.Close += _channelTab_Close;
            this._channelTab.AddChat(new SystemMessageLine("You joined the room."));

            this._channel = channel;
            this._channel.OnMessage += this._channel_OnMessage;
            this._channel.OnJoin += this._channel_OnJoin;
            this._channel.OnLeave += this._channel_OnLeave;
            this._channel.OnNames += this._channel_OnNames;
        }

        private void AddUser(Channel channel, User user)
        {
            UserItem userItem = user.ToUserItem(_channel);
            this._channelTab.AddUser(userItem);

            new IRCChannelUser(this._ircClient, userItem, user, channel);
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
            Run(this.Dispose);
        }

        private void _channel_OnNames(Channel source, string[] users)
        {
            Run(() =>
            {
                foreach (var user in this._channel.Users.Values)
                {
                    this.AddUser(this._channel, user);
                }
            });
        }

        private void _channel_OnLeave(Channel source, User user)
        {
            Run(() => this._channelTab.RemoveUser(user.ToUserItem(source)));
        }

        private void _channel_OnJoin(Channel source, User user)
        {
            Run(() => this.AddUser(this._channel, user));
        }

        private void _channel_OnMessage(Channel source, User user, string message)
        {
            Run(() => this._channelTab.AddChat(user, _channel, message));
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
        }

        #endregion
    }
}
