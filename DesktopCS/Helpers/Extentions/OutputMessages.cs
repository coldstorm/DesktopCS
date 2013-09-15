using System;
using DesktopCS.Helpers.Parsers;
using DesktopCS.Models;
using DesktopCS.Services.Command;
using NetIRC;

namespace DesktopCS.Helpers.Extentions
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
            tab.AddChat(user, MIRCHelper.ItalicChar + message + MIRCHelper.ItalicChar);
        }

        public static void AddAction(this Tab tab, UserItem user, string message)
        {
            tab.AddChat(new MessageLine(user, MIRCHelper.ItalicChar + message + MIRCHelper.ItalicChar));
        }

        #region Channel

        public static void AddJoin(this Tab tab, string nick)
        {
            tab.AddSystemChat(String.Format("{0} joined the room.", nick));
        }

        public static void AddJoin(this Tab tab)
        {
            tab.AddSystemChat(String.Format("You joined the room."));
        }

        public static void AddLeave(this Tab tab, string nick, string reason)
        {
            tab.AddSystemChat(!String.IsNullOrEmpty(reason)
                ? String.Format("{0} left the room ({1}).", nick, reason)
                : String.Format("{0} left the room.", nick));
        }

        public static void AddLeave(this Tab tab, string reason)
        {
            tab.AddSystemChat(!String.IsNullOrEmpty(reason)
                ? String.Format("You left the room ({0}).", reason)
                : String.Format("You left the room."));
        }

        public static void AddTopic(this Tab tab, string nick)
        {
            tab.AddSystemChat(String.Format("Topic was changed by {0}.", nick));
        }

        public static void AddMode(this Tab tab, string from, string to, UserRank rank)
        {
            string rankStr = rank.ToString().ToLower();
            tab.AddSystemChat(String.Format("{0} gave {2} to {1}.", from, to, rankStr));
        }

        #endregion
        
        #region User

        public static void AddNickChange(this Tab tab, string oldNick, string newNick)
        {
            tab.AddSystemChat(String.Format("{0} is now known as {1}.", oldNick, newNick));
        }

        public static void AddNickChange(this Tab tab, string newNick)
        {
            tab.AddSystemChat(String.Format("You are now known as {0}.", newNick));
        }

        public static void AddQuit(this Tab tab, string nick, string reason)
        {
            tab.AddSystemChat(!String.IsNullOrEmpty(reason)
                ? String.Format("{0} quit ({1}).", nick, reason)
                : String.Format("{0} quit.", nick));
        }

        public static void AddQuit(this Tab tab, string reason)
        {
            tab.AddSystemChat(String.Format("You quit ({0}).", reason));
        }

        #endregion

    }
}
