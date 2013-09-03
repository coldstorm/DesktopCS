using System;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.MVVM;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCChannelUser : UIInvoker, IDisposable
    {
        private readonly UserItem _userItem;
        private readonly User _user;
        private readonly Channel _channel;

        public IRCChannelUser(UserItem userItem, User user, Channel channel)
        {
            this._userItem = userItem;

            this._user = user;
            this._user.OnNickNameChange += this._user_OnNickNameChange;
            this._user.OnUserNameChange += this._user_OnUserNameChange;

            this._channel = channel;
            this._channel.OnLeave += this._channel_OnLeave;
        }

        #region Event Handlers

        private void _channel_OnLeave(Channel source, User user)
        {
            Run(() =>
            {
                if (user == this._user)
                {
                    this.Dispose();
                }
            });
        }

        private void _user_OnUserNameChange(User user, string original)
        {
            Run(() => { this._userItem.Metadata = user.ToUserItem().Metadata; });
        }

        private void _user_OnNickNameChange(User user, string original)
        {
            Run(() => { this._userItem.NickName = this._user.NickName; });
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            this._user.OnNickNameChange -= this._user_OnNickNameChange;
            this._user.OnUserNameChange -= this._user_OnUserNameChange;

            this._channel.OnLeave -= this._channel_OnLeave;
        }

        #endregion
    }
}
