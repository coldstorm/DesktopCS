using DesktopCS.Helpers;
using DesktopCS.Helpers.Parsers;

namespace DesktopCS.Models
{
    class SystemMessageLine : ChatLine
    {
        public SystemMessageLine(string message, ParseArgs args)
            : base(ColorHelper.MessageColor, message, args)
        {

        }
    }
}
