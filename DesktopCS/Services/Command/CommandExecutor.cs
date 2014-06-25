using System;
using System.Collections.Generic;
using System.Linq;
using DesktopCS.Controls;
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
            this._commands.Add("JOIN", new Command(new string[] { "JOIN" }, this.JoinCallback, "/join [#channel[,#channel2[,...]]]", 
                "Joins the specified channel(s). If a channel is not specified and the current tab is an unjoined channel, it joins it. Multiple channels need to be comma-separated."));
            this._commands.Add("PART", new Command(new string[] { "PART" }, this.PartCallback, "/part [#channel[,#channel2[,...]]] [reason]", 
                "Parts the specified channel(s). If a channel is not specified and the current tab is a joined channel, it parts it. Multiple channels need to be comma-separated."));

            this._commands.Add("QUERY", new Command(new string[] { "QUERY", "MSG" }, this.QueryCallback, "/query <target> <message>", 
                "Sends the specified message to the target (a user or a channel)."));

            this._commands.Add("AWAY", new Command(new string[] { "AWAY", "AFK" }, this.AwayCallback, "/away [message]", 
                "Marks the user as away with an optional message. If the user is already away, it removes their away status."));
            this._commands.Add("BACK", new Command(new string[] { "BACK", "BACK" }, this.BackCallback, "/back", 
                "Removes the user's away status."));

            this._commands.Add("HELP", new Command(new string[] { "HELP" }, this.HelpCallback, "/help [command]", 
                "Displays help for a specified command. If a command is not specified this help text will display."));
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

        private void HelpCallback(CommandArgs args)
        {
            Tab tab = args.Tab;
            // /help command
            if (args.Parameters.Length >= 1)
            {
                try
                {
                    Command command = this._commands[args.Parameters[0].ToUpperInvariant()];
                    tab.AddHelp(String.Format("{0}: {1}", command.Labels[0], command.Usage), new ParseArgs());
                    return;
                }

                catch (KeyNotFoundException ex)
                {
                    throw InvalidParameter(args.Parameters[0]);
                }
            }
            // /help
            else
            {
                foreach (Command command in this._commands.Values)
                {
                    tab.AddHelp(String.Format("{0}: {1}", command.Labels[0], command.Help), new ParseArgs());
                }
                return;
            }
        }

        private IEnumerable<Channel> GetChannels(string channels)
        {
            return channels.Split(',').Select(channel => new Channel(channel));
        }

        private static CommandException InvalidUsage(Command command)
        {
            return new CommandException("Correct usage: " + command.Usage);
        }

        private static CommandException InvalidParameter(string parameter)
        {
            return new CommandException(String.Format("Invalid parameter: '{0}'", parameter));
        }
    }
}
