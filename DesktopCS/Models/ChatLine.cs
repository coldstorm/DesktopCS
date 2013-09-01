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
        public SolidColorBrush ChatBrush { get; private set; }
        public string Chat { get; private set; }

        public ChatLine(SolidColorBrush chatBrush, string chat, string timestamp)
        {
            this.ChatBrush = chatBrush;
            this.Timestamp = TimeHelper.CreateTimeStamp();
            this.Chat = chat;
            this.Timestamp = timestamp;
        }

        public ChatLine(SolidColorBrush chatBrush, string chat)
        {
            this.ChatBrush = chatBrush;
            this.Timestamp = TimeHelper.CreateTimeStamp();
            this.Chat = chat;
        }

        public ChatLine(UserListItem user, SolidColorBrush chatBrush, string chat)
        {
            this.ChatBrush = chatBrush;
            this.Timestamp = TimeHelper.CreateTimeStamp();
            this.Chat = chat;
            this.User = user;
        }

        public ChatLine(UserListItem user, SolidColorBrush chatBrush, string chat, string timestamp)
        {
            this.ChatBrush = chatBrush;
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
                var timeRun = new Run(this.Timestamp) { Foreground = BrushHelper.TimeBrush };
                p.Inlines.Add(timeRun);
                p.Inlines.Add(" ");
            }

            // User
            if (this.User != null)
            {
                Brush brush = BrushHelper.ChatBrush;
                if (this.User.Metadata != null)
                    brush = this.User.Metadata.Color;

                var usernameRun = new Run(this.User.Nickname) { Foreground = brush };
                p.Inlines.Add(usernameRun);
                p.Inlines.Add(" ");
            }

            // Message
            var chatRun = new Run(this.Chat) { Foreground = this.ChatBrush };
            p.Inlines.Add(chatRun);

            return p;
        }
    }
}
