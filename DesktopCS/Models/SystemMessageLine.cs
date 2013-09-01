using DesktopCS.Helpers;

namespace DesktopCS.Models
{
    class SystemMessageLine : ChatLine
    {
        public SystemMessageLine(string message)
            : base(ColorHelper.MessageColor, message)
        {
        }
    }
}
