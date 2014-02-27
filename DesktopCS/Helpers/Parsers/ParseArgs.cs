using System;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    public class ParseArgs
    {
        public String HostNickname { get; set; }
        public Color Forecolor { get; set; }
        public bool SoundNotification { get; set; }
        public bool DesktopNotification { get; set; }
        public Action<string> QueryCallback { get; set; }

        public ParseArgs()
        {
        }

        public ParseArgs(string hostNickname, bool soundNotification, bool desktopNotification, Action<string> queryCallback)
        {
            this.HostNickname = hostNickname;
            this.SoundNotification = soundNotification;
            this.DesktopNotification = desktopNotification;
            this.QueryCallback = queryCallback;
        }

        public ParseArgs(Color forecolor, bool soundNotification, bool desktopNotification)
        {
            this.Forecolor = forecolor;
            this.SoundNotification = soundNotification;
            this.DesktopNotification = desktopNotification;
        }
    }
}
