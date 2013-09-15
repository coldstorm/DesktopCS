using System;

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
