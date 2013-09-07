using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.MVVM;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCChannelUser : UIInvoker, IDisposable
    {
        private readonly IRCClient _ircClient;
        private readonly UserItem _userItem;
        private readonly User _user;
        private readonly Channel _channel;

        public IRCChannelUser(IRCClient ircClient, UserItem userItem, User user, Channel channel)
        {
            this._ircClient = ircClient;
            this._ircClient.ChannelLeave += this._ircClient_ChannelLeave;

            this._userItem = userItem;

            this._user = user;
            this._user.OnNickNameChange += this._user_OnNickNameChange;
            this._user.OnUserNameChange += this._user_OnUserNameChange;

            this._channel = channel;
            this._channel.OnLeave += this._channel_OnLeave;
            this._channel.OnRank += this._channel_OnRank;
        }

        #region Event Handlers

        private void _ircClient_ChannelLeave(object sender, Channel channel)
        {
            if (channel == this._channel)
            {
                this.Dispose();
            }
        }

        private void _channel_OnRank(Channel source, User user, UserRank rank)
        {
            this.Run(() =>
            {
                if (user == this._user)
                {
                    this._userItem.Rank = rank;
                }
            });
        }

        private void _channel_OnLeave(Channel source, User user)
        {
            this.Run(() =>
            {
                if (user == this._user)
                {
                    this.Dispose();
                }
            });
        }

        private void _user_OnUserNameChange(User user, string original)
        {
            this.Run(() => { this._userItem.Metadata = user.ToUserItem(this._channel).Metadata; });
        }

        private void _user_OnNickNameChange(User user, string original)
        {
            this.Run(() => { this._userItem.NickName = this._user.NickName; });
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            this._ircClient.ChannelLeave -= this._ircClient_ChannelLeave;

            this._user.OnNickNameChange -= this._user_OnNickNameChange;
            this._user.OnUserNameChange -= this._user_OnUserNameChange;

            this._channel.OnRank -= this._channel_OnRank;
            this._channel.OnLeave -= this._channel_OnLeave;
        }

        #endregion
    }
}
