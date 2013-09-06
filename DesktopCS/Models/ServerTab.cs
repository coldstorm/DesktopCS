using System.Windows.Documents;
using DesktopCS.Controls;

namespace DesktopCS.Models
{
    public class ServerTab : Tab
    {
        public ServerTab(string header, FlowDocument flowDoc, CSTabItem tabItem)
            : base(header, flowDoc, tabItem)
        {
            this.IsClosable = false;
        }

        public override void MarkUnread()
        {
            // Ignore the request
        }
    }
}
