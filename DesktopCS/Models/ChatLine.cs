using System;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Helpers;
using DesktopCS.Helpers.Parsers;

namespace DesktopCS.Models
{
    public class ChatLine
    {
        public string Timestamp { get; private set; }
        public UserItem User { get; private set; }
        public Color ChatColor { get; private set; }
        public string Chat { get; private set; }
        public ParseArgs Args { get; private set; }

        public ChatLine(Color chatColor, string chat, string timestamp, ParseArgs args)
        {
            this.ChatColor = chatColor;
            this.Timestamp = timestamp;
            this.Chat = chat;
            this.Args = args;
        }

        public ChatLine(UserItem user, Color chatColor, string chat, string timestamp, ParseArgs args)
        {
            this.ChatColor = chatColor;
            this.Timestamp = timestamp;
            this.Chat = chat;
            this.User = user;
            this.Args = args;
        }

        public ChatLine(Color chatColor, string chat, ParseArgs args)
        {
            this.Timestamp = TimeHelper.CreateTimeStamp();
            this.ChatColor = chatColor;
            this.Chat = chat;
            this.Args = args;
        }

        public ChatLine(UserItem user, Color chatColor, string chat, ParseArgs args)
        {
            this.Timestamp = TimeHelper.CreateTimeStamp();
            this.ChatColor = chatColor;
            this.Chat = chat;
            this.User = user;
            this.Args = args;
        }
    }
}
