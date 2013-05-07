using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIRC;

namespace DesktopCS
{
    public enum CommandReturn
    {
        UNKNOWN_COMMAND,
        INSUFFICIENT_PARAMS,
        SUCCESS
    }

    public static class CommandExecutor
    {
        public static CommandReturn Execute(Client client, string command)
        {
            string[] parts = command.Remove(0, 1).ToLowerInvariant().Trim().Split(' ');

            switch (parts[0])
            {
                case "join":
                    if (parts.Length >= 2)
                    {
                        client.JoinChannel(parts[1]);
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
