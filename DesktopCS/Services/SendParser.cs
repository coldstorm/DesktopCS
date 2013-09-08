using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.Helpers;
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

        public ISendMessage Parse(string selectedItem, string message)
        {
            var parsedMessage = new ParsedMessage(this._client, message);

            switch (parsedMessage.Command.ToUpper())
            {
                case "JOIN":
                    if (parsedMessage.Parameters.Length >= 1)
                    {
                        return new Join(new Channel(parsedMessage.Parameters[0]));
                    }
                    if (NetIRCHelper.IsChannel(selectedItem))
                    {
                        return new Join(new Channel(selectedItem));
                    }
                    break;

                case "PART":
                    if (parsedMessage.Parameters.Length >= 2)
                    {
                        string reason = String.Join(" ", parsedMessage.Parameters.Skip(1));
                        return new Part(new Channel(parsedMessage.Parameters[0]), reason);
                    }
                    if (parsedMessage.Parameters.Length == 1)
                    {
                        return new Part(new Channel(parsedMessage.Parameters[0]));
                    }
                    if (NetIRCHelper.IsChannel(selectedItem))
                    {
                        return new Part(new Channel(selectedItem));
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

                case "QUERY": case "MSG":
                    if (parsedMessage.Parameters.Length >= 2)
                    {
                        string text = String.Join(" ", parsedMessage.Parameters.Skip(1));
                        return new UserPrivate(parsedMessage.Parameters[0], text);
                    }
                    break;
            }

            return new Raw(message);
        }
    }
}
