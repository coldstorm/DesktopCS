using DesktopCS.Helpers;

namespace DesktopCS.Models
{
    public class ErrorLine : ChatLine
    {
        public ErrorLine(string error)
            : base(ColorHelper.WarningColor, error)
        {
            
        }
    }
}
