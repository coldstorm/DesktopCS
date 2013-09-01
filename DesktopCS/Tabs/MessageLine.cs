using DesktopCS.Helpers;
using DesktopCS.Models;

namespace DesktopCS.Tabs
{
    class MessageLine : ChatLine
    {
        public MessageLine(UserListItem user, string message)
            : base(user, BrushHelper.ChatBrush, message)
        {
        }
    }
}
