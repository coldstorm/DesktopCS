using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.Models;
using NetIRC;
using NetIRC.Messages.Send;

namespace DesktopCS.Services.IRC
{
    public class IRCUser : IDisposable
    {
        private readonly IRCClient _ircClient;
        private readonly Tab _tab;
        private readonly User _user;

        public IRCUser(IRCClient ircClient, Tab tab, User user)
        {
            this._ircClient = ircClient;
            this._ircClient.Input += _ircClient_Input;

            this._tab = tab;
            this._tab.Close += _tab_Close;

            this._user = user;
            this._user.OnNickNameChange += _user_OnNickNameChange;
        }

        #region Event Handlers

        void _tab_Close(object sender, EventArgs e)
        {
            this.Dispose();
        }

        void _ircClient_Input(object sender, string text)
        {
            if (this._tab.IsSelected)
            {
                this._ircClient.Send(new UserPrivate(this._user, text));
            }
        }

        void _user_OnNickNameChange(User user, string original)
        {
            this._tab.Header = _user.NickName;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this._ircClient.Input -= this._ircClient_Input;

            this._user.OnNickNameChange -= _user_OnNickNameChange;
        }

        #endregion
    }
}
