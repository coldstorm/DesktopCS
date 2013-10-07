using DesktopCS.Helpers.Extensions;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCChannelUser : IRCUserBase
    {
        private readonly IRCClient _ircClient;
        private readonly ChannelTab _tab;
        private readonly UserItem _userItem;
        private readonly User _user;
        private readonly Channel _channel;

        public IRCChannelUser(IRCClient ircClient, ChannelTab tab, UserItem userItem, User user, Channel channel)
            : base(ircClient, tab, user)
        {
            this._ircClient = ircClient;

            this._tab = tab;
            this._ircClient.ChannelLeave += this._ircClient_ChannelLeave;

            this._user = user;
            this._user.OnNickNameChange += this._user_OnNickNameChange;
            this._user.OnUserNameChange += this._user_OnUserNameChange;
            this._user.OnIsAwayChange += this._user_OnIsAwayChange;
            this._user.OnAwayMessageChange += this._user_OnAwayMessageChange;
            this._user.OnQuit += this._user_OnQuit;

            this._channel = channel;
            this._channel.OnLeave += this._channel_OnLeave;
            this._channel.OnRank += this._channel_OnRank;

            this._userItem = userItem;
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

        private void _channel_OnLeave(Channel source, User user, string reason)
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

        private void _user_OnIsAwayChange(User user)
        {
            this.Run(() => { this._userItem.IsAway = this._user.IsAway; });
        }

        private void _user_OnAwayMessageChange(User user, string original)
        {
            this.Run(() => { this._userItem.AwayMessage = this._user.AwayMessage; });
        }

        void _user_OnQuit(User user, string reason)
        {
            this.Run(() => this._tab.RemoveUser(this._user.ToUserItem()));
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();

            this._ircClient.ChannelLeave -= this._ircClient_ChannelLeave;

            this._user.OnNickNameChange -= this._user_OnNickNameChange;
            this._user.OnUserNameChange -= this._user_OnUserNameChange;
            this._user.OnIsAwayChange -= this._user_OnIsAwayChange;

            this._channel.OnRank -= this._channel_OnRank;
            this._channel.OnLeave -= this._channel_OnLeave;
        }

        #endregion
    }
}
