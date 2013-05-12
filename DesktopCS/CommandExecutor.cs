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
        public static CommandReturn Execute(Client client, string command)
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

                        client.JoinChannel(parts[1]);

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

                        client.LeaveChannel(parts[1]);

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
