using System;
using DesktopCS.Helpers.Extensions;
using DesktopCS.Helpers.Parsers;
using DesktopCS.Models;
using NetIRC;

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
            this._ircClient.Message += this._ircClient_Message;
            this._ircClient.Action += this._ircClient_Action;

            this._user = user;
            this._user.OnNickNameChange += this._user_OnNickNameChange;
            this._user.OnQuit += this._user_OnQuit;

            this._tab = tab;
            this._tab.Close += this._tab_Close;
            this._tab.Header = this._user.NickName; // Correct casing
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
                this._tab.AddChat(user, message, new ParseArgs(_ircClient.User.NickName));
            }
        }

        private void _ircClient_Action(object sender, User user, string message)
        {
            if (user == this._user)
            {
                this._tab.AddAction(user, message, new ParseArgs(_ircClient.User.NickName));
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

            this._ircClient.Message -= this._ircClient_Message;
            this._ircClient.Action -= this._ircClient_Action;

            this._tab.Close -= this._tab_Close;

            this._user.OnNickNameChange -= this._user_OnNickNameChange;
            this._user.OnQuit -= this._user_OnQuit;
        }

        #endregion
    }
}
