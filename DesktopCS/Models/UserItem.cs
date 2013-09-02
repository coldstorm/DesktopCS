using System;

namespace DesktopCS.Models
{
    public class UserItem
    {
        public UserItem(string nick, UserMetadata metadata)
        {
            this.Nickname = nick;
            this.Metadata = metadata;
        }

        public string Nickname { get; private set; }
        public UserMetadata Metadata { get; private set; }

        public static bool operator ==(UserItem x, UserItem y)
        {
            if (ReferenceEquals(null, x) || ReferenceEquals(null, y))
                return ReferenceEquals(x, y);;

            return  x.Nickname == y.Nickname;
        }
        public static bool operator !=(UserItem x, UserItem y)
        {
            return !(x == y);
        }

        #region Equality members

        public bool Equals(UserItem other)
        {
            return string.Equals(this.Nickname, other.Nickname);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserItem && Equals((UserItem)obj);
        }

        public override int GetHashCode()
        {
            return (this.Nickname != null ? this.Nickname.GetHashCode() : 0);
        }

        #endregion
    }
}
