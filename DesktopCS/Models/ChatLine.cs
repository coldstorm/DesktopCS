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
            ChatBrush = chatBrush;
            Timestamp = TimeHelper.CreateTimeStamp();
            Chat = chat;
            Timestamp = timestamp;
        }

        public ChatLine(SolidColorBrush chatBrush, string chat)
        {
            ChatBrush = chatBrush;
            Timestamp = TimeHelper.CreateTimeStamp();
            Chat = chat;
        }

        public ChatLine(UserListItem user, SolidColorBrush chatBrush, string chat)
        {
            ChatBrush = chatBrush;
            Timestamp = TimeHelper.CreateTimeStamp();
            Chat = chat;
            User = user;
        }

        public ChatLine(UserListItem user, SolidColorBrush chatBrush, string chat, string timestamp)
        {
            ChatBrush = chatBrush;
            Timestamp = timestamp;
            Chat = chat;
            User = user;
        }

        public Paragraph ToParagraph()
        {
            var p = new Paragraph();

            // Time
            if (!String.IsNullOrEmpty(Timestamp))
            {
                var timeRun = new Run(Timestamp) { Foreground = BrushHelper.TimeBrush };
                p.Inlines.Add(timeRun);
                p.Inlines.Add(" ");
            }

            // User
            if (User != null)
            {
                Brush brush = BrushHelper.ChatBrush;
                if (User.Metadata != null)
                    brush = User.Metadata.Color;

                var usernameRun = new Run(User.Nickname) { Foreground = brush };
                p.Inlines.Add(usernameRun);
                p.Inlines.Add(" ");
            }

            // Message
            var chatRun = new Run(Chat) { Foreground = ChatBrush };
            p.Inlines.Add(chatRun);

            return p;
        }
    }
}
