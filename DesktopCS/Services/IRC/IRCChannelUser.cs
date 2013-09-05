﻿using System;
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
            this._channel.OnRank += _channel_OnRank;
            
        }

        #region Event Handlers

        void _channel_OnRank(Channel source, User user, UserRank rank)
        {
            Run(() =>
            {
                if (user == this._user)
                {
                    this._userItem.Rank = rank;
                }
            });
        }

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
            Run(() => { this._userItem.Metadata = user.ToUserItem(_channel).Metadata; });
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