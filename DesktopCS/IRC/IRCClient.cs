using DesktopCS.Models;
using NetIRC;

namespace DesktopCS
{
    public class IRCClient
    {
        private readonly Client _irc;

        public IRCClient(LoginData loginData)
        {
            _irc = new Client();
            _irc.OnConnect += irc_OnConnect;
            _irc.Connect("frogbox.es", 6667, false, new NetIRC.User(loginData.Username));
        }

        void irc_OnConnect(Client client)
        {
            _irc.JoinChannel("test");
            
        }

    }
}
