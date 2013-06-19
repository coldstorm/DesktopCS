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
        private List<Command> Commands;

        public CommandExecutor()
        {
            Commands.Add(new Command("join", 2, JoinCallback, "/join <channel>"));
            Commands.Add(new Command("part", 2, PartCallback, "/part <channel>"));
            Commands.Add(new Command("topic", 3, TopicCallback, "/topic <channel> <text>"));
        }

        public void Execute(Client invoker, string command)
        {
            string[] parts = command.Remove(0, 1).Trim().Split(' ');

            foreach (Command cmd in Commands)
            {
                if (cmd.Name == parts[0].ToLowerInvariant())
                {
                    cmd.Callback(invoker, parts);
                }
            }
        }

        private void JoinCallback(Client sender, string[] parameters)
        {
            sender.JoinChannel(parameters[1]);
        }

        private void PartCallback(Client sender, string[] parameters)
        {
            sender.LeaveChannel(parameters[1]);
        }

        private void TopicCallback(Client sender, string[] parameters)
        {
            string topic = string.Join(" ", parameters.Skip(2));
            sender.Channels[parameters[1]].SetTopic(topic);
        }
    }
}
