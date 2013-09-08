using System;
using DesktopCS.Models;
using DesktopCS.Services.Command;
using NetIRC;

namespace DesktopCS.Helpers
{
    public static class OutputMessages
    {
        public static void AddException(this Tab tab, CommandException ex)
        {
            tab.AddChat(new ErrorLine(ex.Message));
        }

        public static void AddSystemChat(this Tab tab, string message)
        {
            tab.AddChat(new SystemMessageLine(message));
        }

        public static void AddAction(this Tab tab, User user, string message)
        {
            tab.AddChat(user, String.Format("\0009{0}\0009", message));
        }

        public static void AddAction(this Tab tab, UserItem user, string message)
        {
            tab.AddChat(new MessageLine(user, String.Format("\0009{0}\0009", message)));
        }

        public static void AddNickChange(this Tab tab, string oldNick, string newNick)
        {
            tab.AddSystemChat(String.Format("{0} is now known as {1}.", oldNick, newNick));
        }

        public static void AddQuit(this Tab tab, string nick, string reason)
        {
            tab.AddSystemChat(String.Format("{0} quit ({1}).", nick, reason));
        }
    }
}
