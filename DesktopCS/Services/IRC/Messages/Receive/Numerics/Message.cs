using System;
using System.Linq;
using NetIRC;
using NetIRC.Messages;

namespace DesktopCS.Services.IRC.Messages.Receive.Numerics
{
    public class Message : IReceiveMessage
    {
        private static readonly string[] _blacklist =
        {
            "315", // RPL_ENDOFWHO	
            "331", // RPL_NOTOPIC
            "332", // RPL_TOPIC
            "347", // RPL_ENDOFINVITELIST
            "349", // RPL_ENDOFEXCEPTLIST
            "352", // RPL_WHOREPLY
            "353", // RPL_NAMREPLY
            "366", // RPL_ENDOFNAMES
            "367", // RPL_BANLIST
            "368"  // RPL_ENDOFBANLIST
        };

        public static bool CheckMessage(ParsedMessage message, Client client)
        {
            return message.IsNumeric() &&
                   !_blacklist.Contains(message.Command) &&
                   message.Message.Split(':').Length >= 2;
        }

        #region IReceiveMessage Members

        public void ProcessMessage(ParsedMessage message, Client client)
        {
            User target = message.GetUserFromNick(message.Parameters[0]);

            if (target == client.User)
            {
                string text = String.Join(" ", message.Parameters.Skip(1));
                IRCClient.OnReceiveText(client, text);
            }
        }

        #endregion
    }
}
