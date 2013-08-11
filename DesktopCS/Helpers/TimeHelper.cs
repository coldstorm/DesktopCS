using System;

namespace DesktopCS.Helpers
{
    static class TimeHelper
    {
        public static string CreateTimeStamp()
        {
            return DateTime.Now.ToString("[HH:mm]");
        }
    }
}
