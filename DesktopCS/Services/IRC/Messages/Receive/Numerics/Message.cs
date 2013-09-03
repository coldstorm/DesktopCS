using NetIRC;
using NetIRC.Messages;

namespace DesktopCS.Services.IRC.Messages.Receive.Numerics
{
    public class Message : IReceiveMessage
    {
        public static bool CheckMessage(ParsedMessage message, Client client)
        {
            return message.IsNumeric() &&
                   message.Parameters.Length == 2 &&
                   message.Message.Split(':').Length >= 2;
        }

        #region IReceiveMessage Members

        public void ProcessMessage(ParsedMessage message, Client client)
        {
            User target = message.GetUserFromNick(message.Parameters[0]);

            if (target == client.User)
            {
                IRCClient.OnReceiveText(client, message.Parameters[1]);
            }
        }

        #endregion
    }
}
