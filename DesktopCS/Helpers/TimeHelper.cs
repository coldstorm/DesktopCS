using System;

namespace DesktopCS.Helpers
{
    public static class TimeHelper
    {
        public static string CreateTimeStamp()
        {
            return DateTime.Now.ToString("[HH:mm]");
        }
    }
}
