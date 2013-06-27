using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIRC;

namespace DesktopCS
{
    public class CommandArgs
    {
        public string[] Parameters;
        public ChatOutput Output;

        public CommandArgs(string[] parameters, ChatOutput output)
        {
            Parameters = parameters;
            Output = output;
        }
    }
}
