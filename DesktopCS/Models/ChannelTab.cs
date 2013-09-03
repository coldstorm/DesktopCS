using System.Collections.ObjectModel;
using DesktopCS.MVVM;

namespace DesktopCS.Models
{
    public class ChannelTab : Tab
    {
        public ObservableCollection<UserItem> Users { get; private set; }

        public ChannelTab(string header) : base(header)
        {
            Users = new SortedObservableCollection<UserItem>();
        }

        public void AddUser(UserItem user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => this.AddUser(user));
                return;
            }

            this.Users.Add(user);
        }

        public void RemoveUser(UserItem user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => this.RemoveUser(user));
                return;
            }

            this.Users.Remove(user);
        }
    }
}
