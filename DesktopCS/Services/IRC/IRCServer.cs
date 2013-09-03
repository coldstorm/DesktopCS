using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.Services.IRC.Messages.Send;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCServer
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
            this._client.OnNotice += this.ClientOnNotice;
        }

        void _ircClient_OnText(object sender, string text)
        {
            this.ShowInServer(text);
        }

        private void _ircClient_Input(object sender, string text)
        {
            if (!this._serverTab.IsSelected)
            {
                this.ShowInActive(this._client.User, text);
            }
        }

        private void ClientOnNotice(Client client, User source, string notice)
        {
            if (source != null)
            {
                this.ShowInActive(source, notice);
            }
            else
            {
                this.ShowInServer(notice);
            }
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
    }
}
