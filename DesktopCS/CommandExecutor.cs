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
        private Dictionary<string, Command> Commands;

        public CommandExecutor()
        {
            Commands.Add("join", new Command(2, JoinCallback, "/join <channel>"));
            Commands.Add("part", new Command(2, PartCallback, "/part <channel>"));
            Commands.Add("topic", new Command(3, TopicCallback, "/topic <channel> <text>"));
        }

        public void Execute(Client invoker, string command)
        {
            string[] parts = command.Remove(0, 1).Trim().Split(' ');

            foreach (KeyValuePair<string, Command> entry in Commands)
            {
                if (entry.Key == parts[0].ToLowerInvariant())
                {
                    entry.Value.Callback(invoker, parts);
                }
            }
        }

        private void JoinCallback(Client sender, string[] parameters)
        {
            if (parameters.Length < Commands["join"].MinParams)
            {
                throw new ParameterException();
            }

            sender.JoinChannel(parameters[1]);
        }

        private void PartCallback(Client sender, string[] parameters)
        {
            if (parameters.Length < Commands["part"].MinParams)
            {
                throw new ParameterException();
            }

            sender.LeaveChannel(parameters[1]);
        }

        private void TopicCallback(Client sender, string[] parameters)
        {
            if (parameters.Length < Commands["topic"].MinParams)
            {
                throw new ParameterException();
            }

            string topic = string.Join(" ", parameters.Skip(2));
            sender.Channels[parameters[1]].SetTopic(topic);
        }
    }
}
