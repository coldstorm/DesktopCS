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

        public Paragraph ToParagraph()
        {
            var p = new Paragraph();

            // Time
            if (!String.IsNullOrEmpty(this.Timestamp))
            {
                Color timeColor = ColorHelper.TimeColor;

                var timeRun = new Run(this.Timestamp) { Foreground = new SolidColorBrush(timeColor) };
                p.Inlines.Add(timeRun);
                p.Inlines.Add(" ");
            }

            // User
            if (this.User != null)
            {
                Color color = this.User.Metadata.Color;

                var usernameRun = new Run(this.User.NickName) { Foreground = new SolidColorBrush(color) };
                p.Inlines.Add(usernameRun);
                p.Inlines.Add(" ");
            }

            // Message
            {
                Color color = this.ChatColor;

                Span chatSpan = OutputHelper.Parse(this.Chat, color, this.Args);
                p.Inlines.Add(chatSpan);
            }

            return p;
        }
    }
}
