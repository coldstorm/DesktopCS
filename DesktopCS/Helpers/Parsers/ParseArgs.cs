using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.Helpers.Parsers
{
    public class ParseArgs
    {
        public String HostNickname { get; private set; }

        public ParseArgs(String hostNickname)
        {
            this.HostNickname = hostNickname;
        }
    }
}
