using System;

namespace DesktopCS.Helpers.Parsers
{
    public class ParseArgs
    {
        public String HostNickname { get; private set; }

        public ParseArgs()
        {
        }

        public ParseArgs(string hostNickname)
        {
            this.HostNickname = hostNickname;
        }
    }
}
