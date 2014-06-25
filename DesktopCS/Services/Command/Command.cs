using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.Services.Command
{
    public class Command
    {
        public string[] Labels;
        public int MinParams;
        public Action<CommandArgs> Callback;
        public string Usage;
        public string Help;

        public Command(string[] labels, Action<CommandArgs> callback, string usage, string help)
        {
            this.Labels = labels;
            this.Callback = callback;
            this.Usage = usage;
            this.Help = help;
        }
    }
}
