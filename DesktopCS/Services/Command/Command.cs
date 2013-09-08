using System;
using NetIRC.Messages;

namespace DesktopCS.Services.Command
{
    public class Command
    {
        public int MinParams;
        public Func<CommandArgs, ISendMessage> Callback;
        public string Usage;

        public Command(Func<CommandArgs, ISendMessage> callback, string usage)
        {
            this.Callback = callback;
            this.Usage = usage;
        }
    }
}
