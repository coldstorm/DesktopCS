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
        private delegate BaseTab AddTabDelegate(string title, TabType type);
        private delegate void PopulateUserlistDelegate();
        private AddLineDelegate _addline;
        private AddTabDelegate _addtab;
        private PopulateUserlistDelegate _populateuserlist;

        private Dictionary<UserRank, char?> RankChars = new Dictionary<UserRank, char?>()
        {
            {UserRank.None, null},
            {UserRank.Voice, '+'},
            {UserRank.HalfOp, '#'},
            {UserRank.Op, '@'},
            {UserRank.Admin, '@'},
            {UserRank.Owner, '@'}
        };

        public MainForm()
        {
            InitializeComponent();

            RTF = "{\\rtf{\\colortbl;\\red55\\green78\\blue63;\\red186\\green191\\blue187;}}";

            BackColor = Constants.BACKGROUND_COLOR;
            ForeColor = Constants.TEXT_COLOR;

            MainMenuStrip.BackColor = BackColor;
            MainMenuStrip.ForeColor = ForeColor;

            Userlist.SelectedNode = null;
            Userlist.BackColor = Constants.CHAT_BACKGROUND_COLOR;
            Userlist.ForeColor = ForeColor;

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
            this.Invoke(_addtab, channel.Name, TabType.Channel);
            this.Invoke(_addline, channel.Name, "You joined the channel " + channel.Name);
            this.Invoke(_populateuserlist);
            channel.OnMessage += channel_OnMessage;
        }

        void channel_OnMessage(Channel source, User user, string message)
        {
            this.Invoke(_addline, source.Name, message);
        }

        private BaseTab AddTab(string title, TabType type)
        {
            if (!TabList.TabPages.ContainsKey(title))
            {
                BaseTab Tab = new BaseTab(title);

                //Prepare RichTextBox
                RichTextBox TextBox = new RichTextBox();
                TextBox.Name = "TextBox";
                TextBox.Dock = DockStyle.Fill;
                TextBox.BorderStyle = BorderStyle.None;
                TextBox.BackColor = Constants.CHAT_BACKGROUND_COLOR;
                TextBox.ForeColor = Constants.TEXT_COLOR;
                TextBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                TextBox.ReadOnly = true;

                Tab.Controls.Add(TextBox);
                TabList.TabPages.Add(Tab);

                return Tab;
            }

            return null;
        }

        private void RemoveTab(int index)
        {
            TabList.TabPages.RemoveAt(index);
        }

        private void AddLine(string tabName, string line)
        {
            //initialize the RTF of the RichTextBox in the current tab
            string currRTF;
            if (!String.IsNullOrEmpty((TabList.TabPages[tabName].Controls["TextBox"] as RichTextBox).Rtf))
            {
                currRTF = (TabList.TabPages[tabName].Controls["TextBox"] as RichTextBox).Rtf;
            }

            else
            {
                currRTF = RTF;
            }

            line = line.Trim();
            string newRTF = currRTF;

            //append the new line at the end of the current RTF file
            newRTF = newRTF.Insert(newRTF.LastIndexOf('}'), "\\cf1" + DateTime.Now.ToString("[HH:mm] ") + "\\cf2" + line);
            (TabList.TabPages[tabName].Controls["TextBox"] as RichTextBox).Rtf = newRTF;
        }

        private void AddUser(string username)
        {
            Userlist.Nodes.Add(username);
        }

        private void PopulateUserlist()
        {
            Userlist.Nodes.Clear();

            foreach (User user in Client.Channels[TabList.SelectedTab.Name].Users.Values)
            {
                Userlist.Nodes.Add(RankChars[user.Rank] + user.NickName);
            }
        }

        private void Userlist_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TabList.SelectedTab = AddTab(e.Node.Text, TabType.PrivateMessage);
        }

        private void TabList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((TabList.SelectedTab as BaseTab).Type == TabType.Channel)
            {
                PopulateUserlist();
            }

            else
            {
                Userlist.Nodes.Clear();
            }
        }
    }
}
