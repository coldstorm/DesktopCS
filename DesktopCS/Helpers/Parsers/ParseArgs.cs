using System;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    public class ParseArgs
    {
        public String HostNickname { get; set; }
        public Color Forecolor { get; set; }
        public bool PingSound { get; set; }
        public Action<string> QueryCallback { get; set; }

        public ParseArgs()
        {
        }

        public ParseArgs(string hostNickname, bool pingSound, Action<string> queryCallback)
        {
            this.HostNickname = hostNickname;
            this.PingSound = pingSound;
            this.QueryCallback = queryCallback;
        }

        public ParseArgs(Color forecolor, bool pingSound)
        {
            this.Forecolor = forecolor;
            this.PingSound = pingSound;
        }
    }
}
