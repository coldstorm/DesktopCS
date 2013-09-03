using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.MVVM;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCServer : UIInvoker
    {
        private readonly Tab _serverTab;
        private readonly IRCClient _ircClient;
        private readonly Client _client;

        public IRCServer(IRCClient ircClient, Tab serverTab, Client client)
        {
            this._ircClient = ircClient;
            this._ircClient.Input += this._ircClient_Input;
            this._ircClient.Text += this._ircClient_OnText;

            this._serverTab = serverTab;

            this._client = client;
            this._client.OnNotice += this._client_OnNotice;
        }

        private void ShowInActive(User user, string text)
        {
            Tab selectedTab = this._ircClient.SelectedTab ?? this._serverTab;
            selectedTab.AddChat(user, u => new MessageLine(u, text));
        }

        private void ShowInServer(string text)
        {
            this._serverTab.AddChat(null, u => new MessageLine(u, text));
        }

        #region Event Handlers

        private void _ircClient_OnText(object sender, string text)
        {
            Run(() => this.ShowInServer(text));
        }

        private void _ircClient_Input(object sender, string text)
        {
            Run(() =>
            {
                if (!this._serverTab.IsSelected)
                {
                    this.ShowInActive(this._client.User, text);
                }
            });
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

    }
}
