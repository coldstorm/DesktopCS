using System;
using NetIRC.Messages;

namespace DesktopCS.Services.Command
{
    public class Command
    {
        public string[] Labels;
        public int MinParams;
        public Func<CommandArgs, ISendMessage> Callback;
        public string Usage;
        public string Help;

        public Command(string[] labels, Func<CommandArgs, ISendMessage> callback, string usage, string help)
        {
            this.Labels = labels;
            this.Callback = callback;
            this.Usage = usage;
            this.Help = help;
        }
    }
}
