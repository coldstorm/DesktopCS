using DesktopCS.Services;

namespace DesktopCS.Models
{
    class ServerTab : Tab
    {
        public ServerTab(string header) : base(header)
        {
            TabItem.IsClosable = false;
        }
    }
}
