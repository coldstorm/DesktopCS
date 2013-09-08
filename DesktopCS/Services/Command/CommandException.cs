using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.Services.Command
{
    public class CommandException : Exception
    {
        public CommandException()
        {
        }
        public CommandException(string message) : base(message) { }
    }
}
