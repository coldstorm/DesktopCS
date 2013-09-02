using DesktopCS.Helpers;

namespace DesktopCS.Models
{
    class MessageLine : ChatLine
    {
        public MessageLine(UserItem user, string message)
            : base(user, ColorHelper.ChatColor, message)
        {
        }
    }
}
