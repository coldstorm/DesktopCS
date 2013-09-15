using System;
using System.Collections.Generic;
using System.Linq;
using DesktopCS.Helpers.Extentions;
using DesktopCS.Models;
using DesktopCS.Services.IRC.Messages.Send;
using NetIRC;
using NetIRC.Messages;
using NetIRC.Messages.Send;
using NetIRCHelper = DesktopCS.Helpers.NetIRCHelper;

namespace DesktopCS.Services.Command
{
    public class CommandExecutor
    {
        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>(StringComparer.OrdinalIgnoreCase);

        public CommandExecutor()
        {
            this._commands.Add("JOIN", new Command(this.JoinCallback, "/join [#channel[,#channel2[,...]]]"));
            this._commands.Add("PART", new Command(this.PartCallback, "/part [#channel[,#channel2[,...]]] [reason]"));

            this._commands.Add("QUERY", new Command(this.QueryCallback, "/msg <user> <message>"));

            this._commands.Add("AWAY", new Command(this.AwayCallback, "/away [message]"));
            this._commands.Add("BACK", new Command(this.BackCallback, "/back"));
        }

        public ISendMessage Execute(Client client, Tab tab, string message)
        {
            var parsedMessage = new ParsedMessage(null, message);
            string command = parsedMessage.Command.ToUpperInvariant();

            foreach (var entry in this._commands)
            {
                if (entry.Key == command)
                {
                    try
                    {
                        return entry.Value.Callback(new CommandArgs(client, tab, parsedMessage.Parameters));
                    }
                    catch (CommandException ex)
                    {
                        OutputMessages.AddException(tab, ex);
                        return null;
                    }
                }
            }

            return new Raw(message);
        }
        
        private ISendMessage JoinCallback(CommandArgs arg)
        {
            // /join #channel
            if (arg.Parameters.Length >= 1)
            {
                return new SendCollection(
                    this.GetChannels(arg.Parameters[0])
                        .Select(channel => new Join(channel)));
            }
            // /join
            if (NetIRCHelper.IsChannel(arg.Tab.Header))
            {
                return new Join(new Channel(arg.Tab.Header));
            }

            throw new CommandException("/JOIN expects a channel");
        }

        private ISendMessage PartCallback(CommandArgs arg)
        {
            // /part #channel reason
            if (arg.Parameters.Length >= 2)
            {
                string reason = String.Join(" ", arg.Parameters.Skip(1));
                return new SendCollection(
                    this.GetChannels(arg.Parameters[0])
                        .Select(channel => new Part(channel, reason)));
            }
            // /part #channel
            if (arg.Parameters.Length == 1)
            {
                return new SendCollection(
                    this.GetChannels(arg.Parameters[0])
                        .Select(channel => new Part(channel)));
            }
            // /part
            if (NetIRCHelper.IsChannel(arg.Tab.Header))
            {
                return new Part(new Channel(arg.Tab.Header));
            }

            throw new CommandException("/PART expects a channel");
        }

        private ISendMessage QueryCallback(CommandArgs arg)
        {
            // /query user message
            if (arg.Parameters.Length >= 2)
            {
                string text = String.Join(" ", arg.Parameters.Skip(1));
                return new UserPrivate(arg.Parameters[0], text);
            }

            throw InvalidUsage(this._commands["QUERY"]);
        }

        private ISendMessage AwayCallback(CommandArgs arg)
        {
            // /away message
            if (arg.Parameters.Length >= 1)
            {
                return new Away(String.Join(" ", arg.Parameters));
            }
            // /away
            if (!arg.Client.User.IsAway)
            {
                return new Away("AFK");
            }
            return new NotAway();
        }

        private ISendMessage BackCallback(CommandArgs arg)
        {
            // /back
            return new NotAway();
        }

        private IEnumerable<Channel> GetChannels(string channels)
        {
            return channels.Split(',').Select(channel => new Channel(channel));
        }

        private static CommandException InvalidUsage(Command command)
        {
            return new CommandException("Invalid usage:" + command.Usage);
        }
    }
}
