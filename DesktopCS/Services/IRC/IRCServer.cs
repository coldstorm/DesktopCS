using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.MVVM;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCServer : IRCBase
    {
        private readonly Tab _tab;
        private readonly IRCClient _ircClient;
        private readonly Client _client;

        public IRCServer(IRCClient ircClient, Tab tab, Client client)
            : base(ircClient, tab)
        {
            this._ircClient = ircClient;
            this._ircClient.Input += this._ircClient_Input;
            this._ircClient.Text += this._ircClient_Text;

            this._tab = tab;

            this._client = client;
            this._client.OnNotice += this._client_OnNotice;
        }

        private void ShowInActive(User user, string text)
        {
            Tab selectedTab = this._ircClient.SelectedTab ?? this._tab;
            selectedTab.AddChat(user, text);
        }

        private void ShowInServer(string text)
        {
            this._tab.AddChat(text);
        }

        #region Event Handlers

        private void _ircClient_Text(object sender, string text)
        {
            this.ShowInServer(text);
        }

        private void _ircClient_Input(object sender, string text)
        {
            if (!this._tab.IsSelected)
            {
                this.ShowInActive(this._client.User, text);
            }
        }

        private void _client_OnNotice(Client client, User source, string notice)
        {
            Run(() =>
            {
                if (source != null)
                {
                    this.ShowInActive(source, notice);
                }
                else
                {
                    this.ShowInServer(notice);
                }
            });
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();

            this._ircClient.Input -= this._ircClient_Input;
            this._ircClient.Text -= this._ircClient_Text;

            this._client.OnNotice -= this._client_OnNotice;
        }

        #endregion
    }
}
