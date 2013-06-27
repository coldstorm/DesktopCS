using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIRC;
using DesktopCS.Forms;

namespace DesktopCS
{
    public class CommandExecutor
    {
        private Dictionary<string, Command> Commands = new Dictionary<string,Command>();

        private readonly List<char> ChannelChars = new List<char>() { '#', '&', '!', '+' };

        public CommandExecutor()
        {
            Commands.Add("me", new Command(2, MeCallback, "/me <action>"));

            Commands.Add("join", new Command(2, JoinCallback, "/join <channel>"));
            Commands.Add("part", new Command(2, PartCallback, "/part <channel>"));

            Commands.Add("topic", new Command(2, TopicCallback, "/topic <text>"));
            Commands.Add("kick", new Command(2, KickCallback, "/kick <user> [message]"));

            Commands.Add("clear", new Command(1, ClearCallback, "/clear"));

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
                        entry.Value.Callback(invoker, new CommandArgs(parts, output));
                    }

                    catch (CommandException ex)
                    {
                        output.AddInfoLine(ex.Message);
                    }
                    return;
                }
            }

            output.AddInfoLine("Unknown command.");
        }

        private void MeCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["me"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            string action = string.Join(" ", args.Parameters.Skip(1));

            if (args.Output.Tab.Type == Forms.TabType.Channel)
            {
                Channel channel = (args.Output.Tab as ChannelTab).Channel;
                sender.Send(new NetIRC.Messages.Send.CTCP.ActionMessage(channel, action));
            }

            else
            {
                args.Output.AddInfoLine("This command can only be used in a channel.");
            }
        }

        private void JoinCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["join"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            if (ChannelChars.Contains(args.Parameters[1][0]))
                args.Parameters[1] = args.Parameters[1].Remove(0, 1);

            sender.JoinChannel(args.Parameters[1]);
        }

        private void PartCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["part"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            if (ChannelChars.Contains(args.Parameters[1][0]))
                args.Parameters[1] = args.Parameters[1].Remove(0, 1);

            sender.LeaveChannel(args.Parameters[1]);
        }

        private void TopicCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["topic"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }


            if (args.Output.Tab.Type == TabType.Channel)
            {
                Channel channel = (args.Output.Tab as ChannelTab).Channel;
                string topic = string.Join(" ", args.Parameters.Skip(1));

                sender.Send(channel.SetTopic(topic));
            }

            else
            {
                args.Output.AddInfoLine("This command can only be used in a channel.");
            }
        }

        private void KickCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["kick"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            if (args.Output.Tab.Type == TabType.Channel)
            {
                Channel channel = (args.Output.Tab as ChannelTab).Channel;
                if (channel.Users.ContainsKey(args.Parameters[1]))
                {
                    if (args.Parameters.Length > 2)
                    {
                        string message = string.Join(" ", args.Parameters.Skip(2));
                        sender.Send(channel.Kick(channel.Users[args.Parameters[1]], message));
                    }

                    else
                    {
                        sender.Send(channel.Kick(channel.Users[args.Parameters[1]]));
                    }
                }

                else
                {
                    args.Output.AddInfoLine("The user is not in this channel.");
                }
            }

            else
            {
                args.Output.AddInfoLine("This command can only be used in a channel.");
            }
        }

        private void ClearCallback(Client sender, CommandArgs args)
        {
            args.Output.RemoveAll();
            args.Output.Tab.LineID = 0;
        }

        private void HelpCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["help"].MinParams)
            {
                foreach (KeyValuePair<string, Command> pair in Commands)
                {
                    args.Output.AddInfoLine(String.Format("{0}: {1}", pair.Key, pair.Value.Usage));
                }
                return;
            }

            args.Output.AddInfoLine("Usage: " + Commands[args.Parameters[1]].Usage);
        }
    }
}
