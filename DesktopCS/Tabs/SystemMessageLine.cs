using DesktopCS.Helpers;

namespace DesktopCS.Tabs
{
    class SystemMessageLine : ChatLine
    {
        public SystemMessageLine(string message)
            : base(BrushHelper.MessageBrush, message)
        {
        }
    }
}
