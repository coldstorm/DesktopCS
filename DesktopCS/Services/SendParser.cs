using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.Services.IRC.Messages.Send;
using NetIRC;
using NetIRC.Messages;
using NetIRC.Messages.Send;

namespace DesktopCS.Services
{
    public class SendParser
    {
        public ISendMessage Parse(string message)
        {
            var parsedMessage = new ParsedMessage(null, message);

            switch (parsedMessage.Command.ToUpper())
            {
                case "JOIN":
                    if (parsedMessage.Parameters.Length >= 1)
                    {
                        return new Join(new Channel(parsedMessage.Parameters[0]));
                    }
                    break;

                case "AWAY":
                    if (parsedMessage.Parameters.Length >= 1)
                    {
                        return new Away(parsedMessage.Parameters[0]);
                    }
                    break;

                case "BACK":
                    return new NotAway();
            }

            return new Raw(message);
        }
    }
}
