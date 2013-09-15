using System;
using DesktopCS.Helpers;
using DesktopCS.Helpers.Extentions;
using DesktopCS.Models;
using NetIRC;
using NetIRC.Messages.Send;

namespace DesktopCS.Services.IRC
{
    public class IRCUser : IRCUserBase
    {
        private readonly IRCClient _ircClient;
        private readonly Tab _tab;
        private readonly User _user;

        public IRCUser(IRCClient ircClient, Tab tab, User user)
            : base(ircClient, tab, user)
        {
            this._ircClient = ircClient;
            this._ircClient.Input += this._ircClient_Input;
            this._ircClient.Message += this._ircClient_Message;
            this._ircClient.Action += this._ircClient_Action;

            this._tab = tab;
            this._tab.Close += this._tab_Close;

            this._user = user;
            this._user.OnNickNameChange += this._user_OnNickNameChange;
            this._user.OnQuit += this._user_OnQuit;
        }

        #region Event Handlers

        private void _tab_Close(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void _ircClient_Message(object sender, User user, string message)
        {
            if (user == this._user)
            {
                this._tab.AddChat(user, message);
            }
        }

        private void _ircClient_Action(object sender, User user, string message)
        {
            if (user == this._user)
            {
                this._tab.AddAction(user, message);
            }
        }

        private void _ircClient_Input(object sender, string text)
        {
            if (this._tab.IsSelected)
            {
                this._ircClient.Send(new UserPrivate(this._user, text));
            }
        }

        private void _user_OnQuit(User user, string reason)
        {
            this.Run(this.Dispose);
        }

        private void _user_OnNickNameChange(User user, string original)
        {
            this.Run(() => { this._tab.Header = this._user.NickName; });
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();

            this._ircClient.Input -= this._ircClient_Input;
            this._ircClient.Message -= this._ircClient_Message;
            this._ircClient.Action -= this._ircClient_Action;

            this._tab.Close -= this._tab_Close;

            this._user.OnNickNameChange -= this._user_OnNickNameChange;
            this._user.OnQuit -= this._user_OnQuit;
        }

        #endregion
    }
}
