using System.ComponentModel;
using DesktopCS.MVVM;

namespace DesktopCS.Models
{
    public class UserItem : UIInvoker, INotifyPropertyChanged
    {
        private string _nickName;
        private UserMetadata _metadata;

        public UserItem(string nick, UserMetadata metadata)
        {
            this.NickName = nick;
            this.Metadata = metadata;
        }

        public string NickName
        {
            get { return this._nickName; }
            set
            {
                this._nickName = value;
                this.OnPropertyChanged("NickName");
            }
        }

        public UserMetadata Metadata
        {
            get { return this._metadata; }
            set
            {
                this._metadata = value;
                this.OnPropertyChanged("Metadata");
            }
        }

        public static bool operator ==(UserItem x, UserItem y)
        {
            if (ReferenceEquals(null, x) || ReferenceEquals(null, y))
                return ReferenceEquals(x, y);;

            return  x.NickName == y.NickName;
        }
        public static bool operator !=(UserItem x, UserItem y)
        {
            return !(x == y);
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => this.OnPropertyChanged(propertyName));
                return;
            }

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Equality members

        public bool Equals(UserItem other)
        {
            return string.Equals(this.NickName, other.NickName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserItem && this.Equals((UserItem)obj);
        }

        public override int GetHashCode()
        {
            return (this.NickName != null ? this.NickName.GetHashCode() : 0);
        }

        #endregion
    }
}
