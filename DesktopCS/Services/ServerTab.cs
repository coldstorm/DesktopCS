namespace DesktopCS.Services
{
    class ServerTab : Tab
    {
        public ServerTab(string header) : base(header)
        {
            this.TabItem.IsClosable = false;
        }
    }
}
