using System;
using DesktopCS.Helpers;
using NetIRC;
using NetIRC.Messages;

namespace DesktopCS.Services.IRC.Messages.Receive.Numerics
{
    class ParamMessage : IReceiveMessage
    {
    
        public static bool CheckMessage(ParsedMessage message, Client client)
        {
            return message.IsNumeric() &&
                   message.Parameters.Length == 3 &&
                   message.Message.Split(':').Length >= 2 &&
                   message.Parameters[1].Length >= 1 &&
                   !NetIRCHelper.TypeChars.ContainsValue(message.Parameters[1][0]);
        }

        #region IReceiveMessage Members

        public void ProcessMessage(ParsedMessage message, Client client)
        {
            User target = message.GetUserFromNick(message.Parameters[0]);

            if (target == client.User)
            {
                IRCClient.OnReceiveText(client, String.Format("{0} {1}", message.Parameters[1], message.Parameters[2]));
            }
        }

        #endregion
    }
}
