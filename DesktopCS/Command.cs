using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIRC;

namespace DesktopCS
{
    public class Command
    {
        public string Name;
        public int MinParams;
        public delegate void callback(Client sender, string[] parameters);
        public callback Callback;
        public string Usage;

        public Command(string name, int minparams, callback cback, string usage)
        {
            Name = name;
            MinParams = minparams;
            this.Callback = cback;
            Usage = usage;
        }
    }
}
