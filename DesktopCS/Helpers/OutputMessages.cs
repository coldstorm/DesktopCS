using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.Models;
using DesktopCS.Services.Command;

namespace DesktopCS.Helpers
{
    public static class OutputMessages
    {
        public static void AddException(this Tab tab, CommandException ex)
        {
            tab.AddChat(new ErrorLine(ex.Message));
        }
    }
}
