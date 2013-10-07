using DesktopCS.Helpers;
using DesktopCS.Helpers.Parsers;

namespace DesktopCS.Models
{
    class MessageLine : ChatLine
    {
        public MessageLine(UserItem user, string message, ParseArgs args)
            : base(user, ColorHelper.ChatColor, message, args)
        {

        }
    }
}
