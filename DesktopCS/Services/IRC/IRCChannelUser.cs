using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCChannelUser : IDisposable
    {
        private readonly UserItem _userItem;
        private readonly User _user;
        private readonly Channel _channel;

        public IRCChannelUser(UserItem userItem, User user, Channel channel)
        {
            this._userItem = userItem;

            this._user = user;
            this._user.OnNickNameChange += _user_OnNickNameChange;
            this._user.OnUserNameChange += _user_OnUserNameChange;

            this._channel = channel;
            this._channel.OnLeave += _channel_OnLeave;
        }

        void _channel_OnLeave(Channel source, User user)
        {
            if (user == this._user)
            {
                this.Dispose();
            }
        }

        void _user_OnUserNameChange(User user, string original)
        {
            _userItem.Metadata = user.ToUserItem().Metadata;
        }

        void _user_OnNickNameChange(User user, string original)
        {
            _userItem.NickName = _user.NickName;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            this._user.OnNickNameChange -= this._user_OnNickNameChange;
            this._user.OnUserNameChange -= this._user_OnUserNameChange;

            this._channel.OnLeave -= this._channel_OnLeave;
        }

        #endregion
    }
}
