using System.Windows.Media;
using DesktopCS.Helpers;

namespace DesktopCS.Models
{
    public class ChatLine
    {
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

        public ChatLine(User user, SolidColorBrush chatBrush, string chat)
        {
            ChatBrush = chatBrush;
            Timestamp = TimeHelper.CreateTimeStamp();
            Chat = chat;
            User = user;
        }

        public ChatLine(SolidColorBrush userBrush, User user, SolidColorBrush chatBrush, string chat, string timestamp)
        {
            ChatBrush = chatBrush;
            Timestamp = timestamp;
            Chat = chat;
            User = user;
        }

        public string Timestamp { get; private set; }
        public User User { get; private set; }
        public SolidColorBrush ChatBrush { get; private set; }
        public string Chat { get; private set; }
    }
}
