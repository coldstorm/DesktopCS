using DesktopCS.Helpers;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS
{
    public class IRCClient
    {
        private readonly TabManager _tabManager;
        private readonly Client _irc;

        public IRCClient(TabManager tabManager, LoginData loginData)
        {
            _tabManager = tabManager;
            _irc = new Client();
            _irc.OnConnect += irc_OnConnect;

            var cc = CountryCodeHelper.GetCC();
            var user = new NetIRC.User(loginData.Username, IdentHelper.Generate(loginData.ColorBrush, cc));
            _irc.Connect("frogbox.es", 6667, false, user);
        }

        void irc_OnConnect(Client client)
        {
            _irc.JoinChannel("test");
            
        }

    }
}
