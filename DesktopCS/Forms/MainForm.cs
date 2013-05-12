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
        private delegate void AddLineWithAuthorDelegate(string tabName, User author, string line);
        private delegate BaseTab AddTabDelegate(BaseTab tab);
        private delegate void PopulateUserlistDelegate();
        private delegate void UpdateTopicLabelDelegate();
        private AddLineDelegate _addline;
        private AddLineWithAuthorDelegate _addlinewithauthor;
        private AddTabDelegate _addtab;
        private PopulateUserlistDelegate _populateuserlist;
        private UpdateTopicLabelDelegate _updatetopiclabel;

        public MainForm()
        {
            InitializeComponent();

            RTF = "{\\rtf{\\colortbl\\red55\\green78\\blue63;\\red186\\green191\\blue187;}}";

            BackColor = Constants.BACKGROUND_COLOR;
            ForeColor = Constants.TEXT_COLOR;

            MainMenuStrip.BackColor = BackColor;
            MainMenuStrip.ForeColor = ForeColor;

            InputBox.BackColor = Constants.CHAT_BACKGROUND_COLOR;
            InputBox.ForeColor = ForeColor;

            TopicLabel.BackColor = Constants.BACKGROUND_COLOR;
            TopicLabel.ForeColor = Constants.TOPIC_TEXT_COLOR;
            TopicLabel.Text = "";

            Client = new Client();
            Client.Connect("frogbox.es", 6667, false, new User("DesktopCS"));
            Client.OnConnect += Client_OnConnect;
            Client.OnChannelJoin += Client_OnChannelJoin;

            _addline = new AddLineDelegate(AddLine);
            _addlinewithauthor = new AddLineWithAuthorDelegate(AddLine);
            _addtab = new AddTabDelegate(AddTab);
            _populateuserlist = new PopulateUserlistDelegate(PopulateUserlist);
            _updatetopiclabel = new UpdateTopicLabelDelegate(UpdateTopicLabel);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Client.Disconnect();

            base.OnClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.TopicLabel.MaximumSize = new Size(this.ClientRectangle.Width - 6, 0);

            this.Invalidate();
        }

        #region NetIRC Event Handlers
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
            this.UpdateTopicLabel();

            this.TabList.SwitchToTab(tab.Name);

            channel.OnMessage += channel_OnMessage;
            channel.OnNotice += channel_OnNotice;
            channel.OnJoin += channel_OnJoin;
            channel.OnLeave += channel_OnLeave;
            channel.OnKick += channel_OnKick;
            channel.OnTopicChange += channel_OnTopicChange;

            System.Timers.Timer colorTimer = new System.Timers.Timer();

            colorTimer.Elapsed += (s, e) =>
            {
                colorTimer.Enabled = false;

                this.PopulateUserlist();
            };
            colorTimer.Interval = 1000;

            colorTimer.Enabled = true;
        }

        void channel_OnMessage(Channel source, User user, string message)
        {
            this.AddLine("#" + source.Name, user, message);
        }

        void channel_OnNotice(Channel source, User user, string notice)
        {
            this.AddLine("#" + source.Name, user, notice);
        }

        void channel_OnJoin(Channel source, User user)
        {
            this.AddLine("#" + source.Name, user.NickName + " joined the room.");
            this.PopulateUserlist();

            System.Timers.Timer colorTimer = new System.Timers.Timer();

            colorTimer.Elapsed += (s, e) =>
            {
                colorTimer.Enabled = false;

                this.PopulateUserlist();
            };
            colorTimer.Interval = 1000;

            colorTimer.Enabled = true;
        }

        void channel_OnLeave(Channel source, User user)
        {
            this.AddLine("#" + source.Name, user.NickName + " left the room.");
            this.PopulateUserlist();
        }

        void channel_OnKick(Channel source, User kicker, User user, string reason)
        {
            if (user == Client.User)
            {
                this.AddLine("#" + source.Name, "You were kicked by " + kicker.NickName + " (" + reason + ")");
                UserList.Nodes.Clear();
            }
            else
            {
                this.AddLine("#" + source.Name, kicker.NickName + " kicked " + user.NickName + " (" + reason + ")");
                this.PopulateUserlist();
            }
        }

        void channel_OnTopicChange(Channel source, ChannelTopic topic)
        {
            UpdateTopicLabel();
        }
        #endregion

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

            ChatOutput output = new ChatOutput(this.TabList.Tabs[tabName]);
            output.AddLine(line);
        }

        private void AddLine(string tabName, User author, string line)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_addlinewithauthor, tabName, author, line);
                return;
            }

            ChatOutput output = new ChatOutput(this.TabList.Tabs[tabName]);
            output.AddLine(author, line);

            if (this.TabList.SelectedTab.Name != tabName)
            {
                this.TabList.Tabs[tabName].Active = true;
                this.TabList.Invalidate();
            }
        }

        private void PopulateUserlist()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_populateuserlist);
                return;
            }

            BaseTab selectedTab = TabList.SelectedTab as BaseTab;

            if (selectedTab.Type == TabType.Channel)
            {
                ChannelTab channelTab = selectedTab as ChannelTab;

                this.UserList.PopulateFromChannel(channelTab.Channel);
            }

            else
            {
                UserList.Nodes.Clear();
            }
        }

        private void UpdateTopicLabel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_updatetopiclabel);
                return;
            }

            if ((TabList.SelectedTab as BaseTab).Type == TabType.Channel)
            {
                ChannelTab tab = TabList.SelectedTab as ChannelTab;

                if (tab.Channel.Topic.Message != null && tab.Channel.Topic.Author != null)
                {
                    TopicLabel.Text = tab.Channel.Topic.Message + " (by " + tab.Channel.Topic.Author.NickName + ")";
                }
            }

            else
            {
                TopicLabel.Text = "";
            }
        }

        private void ProcessInput(string input)
        {
            input = input.Trim();
            if (input.StartsWith("/"))
            {
                if ((TabList.SelectedTab as BaseTab).Type == TabType.Channel)
                {
                    if (CommandExecutor.Execute(this.Client, input) == CommandReturn.INSUFFICIENT_PARAMS)
                    {
                        AddLine(TabList.SelectedTab.Name, "Insufficient parameters.");
                    }

                    else if (CommandExecutor.Execute(this.Client, input) == CommandReturn.UNKNOWN_COMMAND)
                    {
                        AddLine(TabList.SelectedTab.Name, "Unknown command.");
                    }
                }
            }

            else
            {
                if ((TabList.SelectedTab as BaseTab).Type == TabType.Channel)
                {
                    Client.Send(new NetIRC.Messages.Send.ChatMessage((TabList.SelectedTab as ChannelTab).Channel, input));
                    AddLine(TabList.SelectedTab.Name, Client.User, input);
                }
            }
        }

        #region Control Event Handlers
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
            PopulateUserlist();
            UpdateTopicLabel();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProcessInput(InputBox.Text);
                InputBox.ResetText();
            }
        }

        private void TopicLabel_ClientSizeChanged(object sender, System.EventArgs e)
        {
            int offset = this.TopicLabel.Bottom + 3;

            this.UserList.Top = offset + 25;
            this.TabList.Top = offset;

            this.TabList.Height = this.Height - this.InputBox.Height - this.TopicLabel.Height - this.MenuStrip.Height - 49;
            this.UserList.Height = this.TabList.Height - 9;

            this.InputBox.Top = this.TabList.Bottom + 3;

            this.TopicLabel.Location = new Point(this.ClientRectangle.Width - this.TopicLabel.Width, this.TopicLabel.Top);

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Rectangle userListRect = this.UserList.Bounds;

            userListRect.X -= 1;
            userListRect.Y -= 1;

            userListRect.Width += 1;
            userListRect.Height += 1;

            g.DrawRectangle(new Pen(Constants.TAB_BORDER_COLOR), userListRect);

            Rectangle inputBoxRectangle = this.InputBox.Bounds;

            inputBoxRectangle.X -= 1;
            inputBoxRectangle.Y -= 1;

            inputBoxRectangle.Width += 1;
            inputBoxRectangle.Height += 1;

            g.DrawRectangle(new Pen(Constants.TAB_BORDER_COLOR), inputBoxRectangle);
        }
        #endregion
    }
}
