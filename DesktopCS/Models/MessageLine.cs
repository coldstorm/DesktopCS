using DesktopCS.Helpers;

namespace DesktopCS.Models
{
    class MessageLine : ChatLine
    {
        public MessageLine(UserListItem user, string message)
            : base(user, BrushHelper.ChatBrush, message)
        {
        }
    }
}
