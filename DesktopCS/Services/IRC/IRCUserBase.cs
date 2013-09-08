using DesktopCS.Models;
using DesktopCS.Helpers;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCUserBase : IRCBase
    {
        private readonly Tab _tab;
        private readonly User _user;

        public IRCUserBase(IRCClient ircClient, Tab tab, User user) 
            : base(ircClient, tab)
        {
            this._tab = tab;

            this._user = user;
            this._user.OnNickNameChange += this._user_OnNickNameChange;
            this._user.OnQuit += this._user_OnQuit;
        }

        #region Event Handlers

        private void _user_OnNickNameChange(User user, string original)
        {
            this.Run(() => this._tab.AddNickChange(original, this._user.NickName));
        }

        private void _user_OnQuit(User user, string reason)
        {
            this.Run(() => this._tab.AddQuit(this._user.NickName, reason));
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();
            
            this._user.OnNickNameChange -= this._user_OnNickNameChange;
            this._user.OnQuit -= this._user_OnQuit;
        }

        #endregion
    }
}
