using System;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    public class ParseArgs
    {
        public String HostNickname { get; set; }
        public Color Forecolor { get; set; }
        public bool PingSound { get; set; }

        public ParseArgs()
        {
        }

        public ParseArgs(string hostNickname)
        {
            this.HostNickname = hostNickname;
            this.PingSound = true;
        }

        public ParseArgs(string hostNickname, bool pingSound)
        {
            this.HostNickname = hostNickname;
            this.PingSound = pingSound;
        }

        public ParseArgs(Color forecolor, bool pingSound)
        {
            this.Forecolor = forecolor;
            this.PingSound = pingSound;
        }
    }
}
