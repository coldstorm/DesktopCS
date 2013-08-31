using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.Tabs
{
    class ServerTab : Tab
    {
        public ServerTab(string header) : base(header)
        {
            TabItem.IsClosable = false;
        }
    }
}
