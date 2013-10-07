using System;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    public class ParseArgs
    {
        public String HostNickname { get; set; }
        public Color Forecolor { get; set; }

        public ParseArgs()
        {
        }

        public ParseArgs(string hostNickname)
        {
            this.HostNickname = hostNickname;
        }

        public ParseArgs(Color forecolor)
        {
            this.Forecolor = forecolor;
        }
    }
}
