﻿using DesktopCS.Helpers.Extensions;
using DesktopCS.Models;
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
            IRCClient.ReceiveText += this.IRCClient_ReceiveText;

            this._tab = tab;

            this._client = client;
            this._client.OnNotice += this._client_OnNotice;
            this._client.OnDisconnect += this._client_OnDisconnect;
        }

        private void ShowInActive(User user, string text)
        {
            Tab selectedTab = this._ircClient.SelectedTab ?? this._tab;
            selectedTab.AddChat(user, text, this.GetArgs());
        }

        private void ShowInActive(string text)
        {
            Tab selectedTab = this._ircClient.SelectedTab ?? this._tab;
            selectedTab.AddChat(text, this.GetArgs());
        }

        private void ShowInServer(string text)
        {
            Tab selectedTab = this._tab;//this._ircClient.SelectedTab ??
            selectedTab.AddChat(text, this.GetArgs(false, false));
        }

        #region Event Handlers

        private void _ircClient_Input(object sender, string text)
        {
            if (!this._tab.IsSelected)
            {
                this.ShowInActive(this._client.User, text);
            }
        }

        private void _client_OnDisconnect(Client client)
        {
            this.Run(this.Dispose);
        }

        private void IRCClient_ReceiveText(object sender, string text)
        {
            this.Run(() => this.ShowInServer(text));
        }

        private void _client_OnNotice(Client client, User source, string notice)
        {
            this.Run(() =>
            {
                if (source != null)
                {
                    this.ShowInActive(source, notice);
                }
                else
                {
                    this.ShowInActive(notice);
                }
            });
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();

            this._ircClient.Input -= this._ircClient_Input;
            IRCClient.ReceiveText -= this.IRCClient_ReceiveText;

            this._client.OnNotice -= this._client_OnNotice;
            this._client.OnDisconnect -= this._client_OnDisconnect;
        }

        #endregion
    }
}
