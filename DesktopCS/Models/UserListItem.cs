namespace DesktopCS.Models
{
    public class UserListItem
    {
        public UserListItem(string nick, UserMetadata metadata)
        {
            this.Nickname = nick;
            this.Metadata = metadata;
        }

        public string Nickname { get; private set; }
        public UserMetadata Metadata { get; private set; }
    }
}
