using System;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Helpers;

namespace DesktopCS.Models
{
    public class ChatLine
    {
        public string Timestamp { get; private set; }
        public UserListItem User { get; private set; }
        public Color ChatColor { get; private set; }
        public string Chat { get; private set; }

        public ChatLine(Color chatColor, string chat, string timestamp)
        {
            this.ChatColor = chatColor;
            this.Timestamp = TimeHelper.CreateTimeStamp();
            this.Chat = chat;
            this.Timestamp = timestamp;
        }

        public ChatLine(Color chatColor, string chat)
        {
            this.ChatColor = chatColor;
            this.Timestamp = TimeHelper.CreateTimeStamp();
            this.Chat = chat;
        }

        public ChatLine(UserListItem user, Color chatColor, string chat)
        {
            this.ChatColor = chatColor;
            this.Timestamp = TimeHelper.CreateTimeStamp();
            this.Chat = chat;
            this.User = user;
        }

        public ChatLine(UserListItem user, Color chatColor, string chat, string timestamp)
        {
            this.ChatColor = chatColor;
            this.Timestamp = timestamp;
            this.Chat = chat;
            this.User = user;
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
                Color color = ColorHelper.ChatColor;
                if (this.User.Metadata != null)
                    color = this.User.Metadata.Color;

                var usernameRun = new Run(this.User.Nickname) { Foreground = new SolidColorBrush(color) };
                p.Inlines.Add(usernameRun);
                p.Inlines.Add(" ");
            }

            // Message
            {
                Color color = this.ChatColor;

                var chatRun = new Run(this.Chat) {Foreground = new SolidColorBrush(color)};
                p.Inlines.Add(chatRun);
            }

            return p;
        }
    }
}
