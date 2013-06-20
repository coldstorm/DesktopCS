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
            Commands.Add("me", new Command(2, MeCallback, "/me <action>"));

            Commands.Add("join", new Command(2, JoinCallback, "/join <channel>"));
            Commands.Add("part", new Command(2, PartCallback, "/part <channel>"));

            Commands.Add("topic", new Command(2, TopicCallback, "/topic <text>"));
            Commands.Add("kick", new Command(2, KickCallback, "/kick <user> [message]"));

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
                        if (output.Tab.Type == Forms.TabType.Channel)
                        {
                            entry.Value.Callback(invoker, new CommandArgs(parts, (output.Tab as Forms.ChannelTab).Channel));
                        }

                        else
                        {
                            entry.Value.Callback(invoker, new CommandArgs(parts, null));
                        }
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

        private void MeCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["me"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            string action = string.Join(" ", args.Parameters.Skip(1));

            if (args.Target != null)
            {
                sender.Send(new NetIRC.Messages.Send.CTCP.ActionMessage(args.Target, action));
            }

            else
            {
                throw new CommandException("This command can only be used in a channel.");
            }
        }

        private void JoinCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["join"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            sender.JoinChannel(args.Parameters[1]);
        }

        private void PartCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["part"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            sender.LeaveChannel(args.Parameters[1]);
        }

        private void TopicCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["topic"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }


            if (args.Target != null)
            {
                string topic = string.Join(" ", args.Parameters.Skip(1));

                sender.Send(args.Target.SetTopic(topic));
            }

            else
            {
                throw new CommandException("This command can only be used in a channel.");
            }
        }

        private void KickCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["kick"].MinParams)
            {
                throw new CommandException("Insufficient parameters.");
            }

            if (args.Target != null)
            {
                if (args.Target.Users.ContainsKey(args.Parameters[1]))
                {
                    if (args.Parameters.Length > 2)
                    {
                        string message = string.Join(" ", args.Parameters.Skip(2));
                        sender.Send(args.Target.Kick(args.Target.Users[args.Parameters[1]], message));
                    }

                    else
                    {
                        sender.Send(args.Target.Kick(args.Target.Users[args.Parameters[1]]));
                    }
                }

                else
                {
                    throw new CommandException("The user is not in this channel.");
                }
            }

            else
            {
                throw new CommandException("This command can only be used in a channel.");
            }
        }

        private void HelpCallback(Client sender, CommandArgs args)
        {
            if (args.Parameters.Length < Commands["help"].MinParams)
            {
                throw new CommandException("Help usage: " + Commands["help"].Usage);
            }

            throw new CommandException("Usage: " + Commands[args.Parameters[1]].Usage);
        }
    }
}
