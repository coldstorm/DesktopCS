using DesktopCS.Helpers;
using DesktopCS.Helpers.Parsers;

namespace DesktopCS.Models
{
    public class ErrorLine : ChatLine
    {
        public ErrorLine(string error)
            : base(ColorHelper.WarningColor, error)
        {
            
        }

        public ErrorLine(string error, ParseArgs args)
            : base(ColorHelper.WarningColor, error, args)
        {

        }
    }
}
