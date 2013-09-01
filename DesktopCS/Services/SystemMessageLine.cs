using DesktopCS.Helpers;
using DesktopCS.Models;

namespace DesktopCS.Services
{
    class SystemMessageLine : ChatLine
    {
        public SystemMessageLine(string message)
            : base(BrushHelper.MessageBrush, message)
        {
        }
    }
}
