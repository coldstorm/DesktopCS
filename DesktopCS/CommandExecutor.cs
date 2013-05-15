using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIRC;

namespace DesktopCS
{
    public static class CommandExecutor
    {
        public static CommandReturn Execute(Client invoker, Channel target, string command)
        {
            string[] parts = command.Remove(0, 1).Trim().Split(' ');

            switch (parts[0].ToLowerInvariant())
            {
                case "join":
                    if (parts.Length >= 2)
                    {
                        if (parts[1][0] == '#')
                        {
                            parts[1] = parts[1].Substring(1);
                        }

                        invoker.JoinChannel(parts[1]);

                        return CommandReturn.SUCCESS;
                    }

                    else
                    {
                        return CommandReturn.INSUFFICIENT_PARAMS;
                    }

                case "part":
                    if (parts.Length >= 2)
                    {
                        if (parts[1][0] == '#')
                        {
                            parts[1] = parts[1].Substring(1);
                        }

                        invoker.LeaveChannel(parts[1]);

                        return CommandReturn.SUCCESS;
                    }

                    else if (target != null)
                    {
                        invoker.LeaveChannel(target.Name);

                        return CommandReturn.SUCCESS;
                    }

                    else
                    {
                        return CommandReturn.INSUFFICIENT_PARAMS;
                    }

                case "motd":
                    if (parts.Length >= 2)
                    {
                        string motd = string.Concat(parts.Skip(1));

                        if (target != null)
                        {
                            invoker.Send(new NetIRC.Messages.Send.TopicMessage("#" + target.Name, motd));
                        }

                        return CommandReturn.SUCCESS;
                    }

                    else
                    {
                        return CommandReturn.INSUFFICIENT_PARAMS;
                    }

                default:
                    return CommandReturn.UNKNOWN_COMMAND;
            }
        }
    }
}
