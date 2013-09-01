using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.Services.IRC
{
    public class IRCServer
    {
        private readonly Tab _serverTab;
        private readonly TabManager _tabManager;
        private readonly Client _client;

        public IRCServer(TabManager tabManager, Client client)
        {
            this._tabManager = tabManager;
            this._serverTab = tabManager.AddServer("Server");

            this._client = client;
            this._client.OnNotice += this.ClientOnNotice;
        }

        void ClientOnNotice(Client client, User source, string notice)
        {
            UserListItem u = null;
            if (source != null)
                u = new UserListItem(source.NickName, IdentHelper.Parse(source.UserName));
            var line = new MessageLine(u, notice);
            this._serverTab.AddChat(line);
        }

        public void Chat(string text)
        {
            var u = new UserListItem(_client.User.NickName, IdentHelper.Parse(_client.User.UserName));
            var line = new MessageLine(u, text);
            this._tabManager.SelectedTab.AddChat(line);
        }
    }
}
