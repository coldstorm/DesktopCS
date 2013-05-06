using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetIRC;

namespace DesktopCS.Forms
{
    public partial class MainForm : Form
    {
        private string RTF;
        private NetIRC.Client Client;

        private delegate void AddLineDelegate(string tabName, string line);
        private delegate BaseTab AddTabDelegate(BaseTab tab);
        private delegate void PopulateUserlistDelegate();
        private AddLineDelegate _addline;
        private AddTabDelegate _addtab;
        private PopulateUserlistDelegate _populateuserlist;

        public MainForm()
        {
            InitializeComponent();

            RTF = "{\\rtf{\\colortbl;\\red55\\green78\\blue63;\\red186\\green191\\blue187;}}";

            BackColor = Constants.BACKGROUND_COLOR;
            ForeColor = Constants.TEXT_COLOR;

            MainMenuStrip.BackColor = BackColor;
            MainMenuStrip.ForeColor = ForeColor;

            InputBox.BackColor = Constants.CHAT_BACKGROUND_COLOR;
            InputBox.ForeColor = ForeColor;

            Client = new Client();
            Client.Connect("frogbox.es", 6667, false, new User("DesktopCS"));
            Client.OnConnect += Client_OnConnect;
            Client.OnChannelJoin += Client_OnChannelJoin;

            _addline = new AddLineDelegate(AddLine);
            _addtab = new AddTabDelegate(AddTab);
            _populateuserlist = new PopulateUserlistDelegate(PopulateUserlist);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Client.Disconnect();

            base.OnClosing(e);
        }

        void Client_OnConnect(Client client)
        {
            Client.JoinChannel("test");
            Client.OnConnect -= Client_OnConnect;
        }

        void Client_OnChannelJoin(Client client, Channel channel)
        {
            ChannelTab tab = new ChannelTab(channel);

            this.AddTab(tab);

            this.AddLine("#" + channel.Name, "You joined the channel " + channel.Name);
            this.PopulateUserlist();

            channel.OnMessage += channel_OnMessage;
            channel.OnJoin += channel_OnJoin;
        }

        void channel_OnJoin(Channel source, User user)
        {
            this.PopulateUserlist();
        }

        void channel_OnMessage(Channel source, User user, string message)
        {
            this.AddLine("#" + source.Name, message);
        }

        private BaseTab AddTab(BaseTab tab)
        {
            if (this.InvokeRequired)
            {
                return (BaseTab)this.Invoke(_addtab, tab);
            }

            if (!TabList.Tabs.ContainsKey(tab.Text))
            {
                TabList.AddTab(tab);

                Size size = this.TabList.ItemSize;
                size.Width = this.TabList.Width - 1;
                this.TabList.ItemSize = size;

                this.TabList.SizeMode = TabSizeMode.Fixed;

                return tab;
            }

            return TabList.Tabs[tab.Text] as BaseTab;
        }

        private void RemoveTab(int index)
        {
            TabList.RemoveTab(TabList.Tabs.ElementAt(index).Value);
        }

        private void AddLine(string tabName, string line)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_addline, tabName, line);
                return;
            }

            //initialize the RTF of the RichTextBox in the current tab
            string currRTF;
            if (!String.IsNullOrEmpty((TabList.Tabs[tabName].Controls["TextBox"] as RichTextBox).Rtf))
            {
                currRTF = (TabList.Tabs[tabName].Controls["TextBox"] as RichTextBox).Rtf;
            }

            else
            {
                currRTF = RTF;
            }

            line = line.Trim();
            string newRTF = currRTF;

            //append the new line at the end of the current RTF file
            newRTF = newRTF.Insert(newRTF.LastIndexOf('}'), "\\cf1" + DateTime.Now.ToString("[HH:mm] ") + "\\cf2" + line);
            (TabList.Tabs[tabName].Controls["TextBox"] as RichTextBox).Rtf = newRTF;
        }

        private void AddUser(string username)
        {
            UserList.Nodes.Add(username);
        }

        private void PopulateUserlist()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_populateuserlist);
                return;
            }

            UserList.Nodes.Clear();

            BaseTab selectedTab = TabList.SelectedTab as BaseTab;

            if (selectedTab.Type == TabType.Channel)
            {
                ChannelTab channelTab = selectedTab as ChannelTab;

                this.UserList.PopulateFromChannel(channelTab.Channel);
            }
        }

        private void UserList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ChannelTab channelTab = this.TabList.SelectedTab as ChannelTab;

            UserNode userNode = this.UserList.SelectedNode as UserNode;
            User user = userNode.User;

            PrivateMessageTab tab = new PrivateMessageTab(user);

            AddTab(tab);

            this.TabList.SwitchToTab(tab.Name);
        }

        private void TabList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((TabList.SelectedTab as BaseTab).Type == TabType.Channel)
            {
                PopulateUserlist();
            }
            else
            {
                UserList.Nodes.Clear();
            }
        }
    }
}
