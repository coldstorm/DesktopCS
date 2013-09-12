using System.Collections.ObjectModel;
using System.Windows.Documents;
using DesktopCS.Controls;
using DesktopCS.MVVM;

namespace DesktopCS.Models
{
    public class ChannelTab : Tab
    {
        public ObservableCollection<UserItem> Users { get; private set; }
        public Topic Topic { get; private set; }

        public ChannelTab(string header, FlowDocument flowDoc, CSTabItem tabItem) 
            : base(header, flowDoc, tabItem)
        {
            this.Users = new SortedObservableCollection<UserItem>();
            this.Topic = new Topic();
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
