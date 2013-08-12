using System.Windows.Media;
using DesktopCS.Helpers;

namespace DesktopCS.Models
{
    class ChatLine
    {
        public ChatLine(SolidColorBrush color, User user, string chat)
        {
            Timestamp = TimeHelper.CreateTimeStamp();
            Chat = chat;
            User = user;
            Color = color;
        }

        public ChatLine(SolidColorBrush color, User user, string chat, string timestamp)
        {
            Timestamp = timestamp;
            Chat = chat;
            User = user;
            Color = color;
        }

        public string Timestamp { get; private set; }
        public SolidColorBrush Color { get; private set; }
        public User User { get; private set; }
        public string Chat { get; private set; }
    }
}
