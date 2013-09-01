using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.Tabs;
using NetIRC;

namespace DesktopCS.IRC
{
    public class IRCServer
    {
        private readonly Tab _serverTab;
        private readonly Client _client;

        public IRCServer(TabManager tabManager, Client client)
        {
            _serverTab = tabManager.AddServer("Server");

            _client = client;
            _client.OnNotice += ClientOnNotice;
        }

        void ClientOnNotice(Client client, User source, string notice)
        {
            UserListItem u = null;
            if (source != null)
                u = new UserListItem(source.NickName, IdentHelper.Parse(source.UserName));
            var line = new MessageLine(u, notice);
            _serverTab.AddChat(line);
        }
    }
}
