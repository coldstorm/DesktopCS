using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIRC;

namespace DesktopCS
{
    public class CommandExecutor
    {
        private Dictionary<string, Command> Commands = new Dictionary<string,Command>();

        public CommandExecutor()
        {
            Commands.Add("join", new Command(2, JoinCallback, "/join <channel>"));
            Commands.Add("part", new Command(2, PartCallback, "/part <channel>"));
            Commands.Add("topic", new Command(3, TopicCallback, "/topic <channel> <text>"));
            Commands.Add("help", new Command(2, HelpCallback, "/help <command>"));
        }

        public void Execute(Client invoker, string command, ChatOutput output)
        {
            string[] parts = command.Remove(0, 1).Trim().Split(' ');

            foreach (KeyValuePair<string, Command> entry in Commands)
            {
                if (entry.Key == parts[0].ToLowerInvariant())
                {
                    try
                    {
                        entry.Value.Callback(invoker, parts);
                    }

                    catch (CommandException ex)
                    {
                        output.AddLine(ex.Message);
                    }
                    return;
                }
            }

            output.AddLine("Unknown command.");
        }

        private void JoinCallback(Client sender, string[] parameters)
        {
            if (parameters.Length < Commands["join"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            sender.JoinChannel(parameters[1]);
        }

        private void PartCallback(Client sender, string[] parameters)
        {
            if (parameters.Length < Commands["part"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            sender.LeaveChannel(parameters[1]);
        }

        private void TopicCallback(Client sender, string[] parameters)
        {
            if (parameters.Length < Commands["topic"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            string topic = string.Join(" ", parameters.Skip(2));

            if (sender.Channels.ContainsKey(parameters[1]))
                sender.Channels[parameters[1]].SetTopic(topic);
        }

        private void HelpCallback(Client sender, string[] parameters)
        {
            if (parameters.Length < Commands["help"].MinParams)
            {
                throw new CommandException("Help usage: " + Commands["help"].Usage);
            }

            throw new CommandException("Usage: " + Commands[parameters[1]].Usage);
        }
    }
}
