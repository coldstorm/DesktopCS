using System.Collections.ObjectModel;
using DesktopCS.MVVM;

namespace DesktopCS.Models
{
    public class ChannelTab : Tab
    {
        public ObservableCollection<UserItem> Users { get; private set; }

        public ChannelTab(string header) : base(header)
        {
            this.Users = new SortedObservableCollection<UserItem>();
        }

        public void AddUser(UserItem user)
        {
            this.Users.Add(user);
        }

        public void RemoveUser(UserItem user)
        {
            this.Users.Remove(user);
        }
    }
}
