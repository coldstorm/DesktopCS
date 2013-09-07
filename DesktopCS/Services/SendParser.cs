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
        private readonly Client _client;

        public SendParser(Client client)
        {
            this._client = client;
        }

        public ISendMessage Parse(string message)
        {
            var parsedMessage = new ParsedMessage(this._client, message);

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
                    if (!this._client.User.IsAway)
                    {
                        return new Away("AFK");
                    }
                    return new NotAway();

                case "BACK":
                    return new NotAway();
            }

            return new Raw(message);
        }
    }
}
