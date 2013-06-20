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
        public Channel Target;

        public CommandArgs(string[] parameters, Channel target)
        {
            Parameters = parameters;
            Target = target;
        }
    }
}
