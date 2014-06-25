using System;
using System.Collections.Generic;
using System.Linq;
using DesktopCS.Helpers;
using DesktopCS.Helpers.Extensions;
using DesktopCS.Helpers.Parsers;
using DesktopCS.Models;
using DesktopCS.Services.IRC.Messages.Send;
using NetIRC;
using NetIRC.Messages;
using NetIRC.Messages.Send;

namespace DesktopCS.Services.Command
{
    public class CommandExecutor
    {
        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>(StringComparer.OrdinalIgnoreCase);

        public CommandExecutor()
        {
            this._commands.Add("JOIN", new Command(new string[] {}, this.JoinCallback, "/join [#channel[,#channel2[,...]]]", ""));
            this._commands.Add("PART", new Command(new string[] {}, this.PartCallback, "/part [#channel[,#channel2[,...]]] [reason]", ""));

            this._commands.Add("QUERY", new Command(new string[] {"MSG"}, this.QueryCallback, "/msg <user> <message>", ""));

            this._commands.Add("AWAY", new Command(new string[] {"AFK"}, this.AwayCallback, "/away [message]", ""));
            this._commands.Add("BACK", new Command(new string[] {"BACK"}, this.BackCallback, "/back", ""));
        }

        public void Execute(Client client, Tab tab, string message)
        {
            var parsedMessage = new ParsedMessage(null, message);
            string command = parsedMessage.Command.ToUpperInvariant();

            foreach (var entry in this._commands)
            {
                if (entry.Key == command || entry.Value.Labels.Contains(command))
                {
                    try
                    {
                        entry.Value.Callback(new CommandArgs(client, tab, parsedMessage.Parameters));
                        return;
                    }
                    catch (CommandException ex)
                    {
                        tab.AddException(ex, new ParseArgs());
                        return;
                    }
                }
            }

            client.Send(new Raw(message));
        }
        
        private void JoinCallback(CommandArgs args)
        {
            Client client = args.Client;
            // /join #channel
            if (args.Parameters.Length >= 1)
            {
                client.Send(new SendCollection(
                    this.GetChannels(args.Parameters[0])
                        .Select(channel => new Join(channel))));
                return;
            }
            // /join
            if (NetIRCHelper.IsChannel(args.Tab.Header) && !args.Client.Channels.ContainsKey(args.Tab.Header))
            {
                client.Send(new Join(new Channel(args.Tab.Header)));
                return;
            }

            throw InvalidUsage(this._commands["JOIN"]);
        }

        private void PartCallback(CommandArgs args)
        {
            Client client = args.Client;
            // /part #channel reason
            if (args.Parameters.Length >= 2)
            {
                string reason = String.Join(" ", args.Parameters.Skip(1));
                client.Send(new SendCollection(
                    this.GetChannels(args.Parameters[0])
                        .Select(channel => new Part(channel, reason))));
                return;
            }
            // /part #channel
            if (args.Parameters.Length == 1)
            {
                client.Send(new SendCollection(
                    this.GetChannels(args.Parameters[0])
                        .Select(channel => new Part(channel))));
                return;
            }
            // /part
            if (NetIRCHelper.IsChannel(args.Tab.Header))
            {
                client.Send(new Part(new Channel(args.Tab.Header)));
                return;
            }

            throw InvalidUsage(this._commands["PART"]);
        }

        private void QueryCallback(CommandArgs args)
        {
            Client client = args.Client;
            // /query user message
            if (args.Parameters.Length >= 2)
            {
                string text = String.Join(" ", args.Parameters.Skip(1));
                string target = args.Parameters[0];

                ISendMessage message;
                if (NetIRCHelper.IsChannel(target))
                    message = new ChannelPrivate(target, text);
                else
                    message = new UserPrivate(target, text);

                client.Send(message);
                return;
            }

            throw InvalidUsage(this._commands["QUERY"]);
        }

        private void AwayCallback(CommandArgs args)
        {
            Client client = args.Client;
            // /away message
            if (args.Parameters.Length >= 1)
            {
                client.Send(new Away(String.Join(" ", args.Parameters)));
                return;
            }
            // /away
            if (!args.Client.User.IsAway)
            {
                client.Send(new Away("AFK"));
                return;
            }

            client.Send(new NotAway());
        }

        private void BackCallback(CommandArgs args)
        {
            Client client = args.Client;
            // /back
            client.Send(new NotAway());
        }

        private IEnumerable<Channel> GetChannels(string channels)
        {
            return channels.Split(',').Select(channel => new Channel(channel));
        }

        private static CommandException InvalidUsage(Command command)
        {
            return new CommandException("Correct usage: " + command.Usage);
        }
    }
}
