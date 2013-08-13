namespace DesktopCS.Models
{
    public class User
    {
        public User(UserRank rank, string nick, string ident, string host, UserMetadata metadata)
        {
            Host = host;
            Nickname = nick;
            Metadata = metadata;
            Ident = ident;
            Rank = rank;
        }

        public UserRank Rank { get; private set; }
        public string Nickname { get; private set; }
        public string Ident { get; private set; }
        public string Host { get; private set; }
        public UserMetadata Metadata { get; private set; }
    }
}
