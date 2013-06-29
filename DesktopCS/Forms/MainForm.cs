using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using NetIRC;

namespace DesktopCS.Forms
{
    public partial class MainForm : Form
    {
        internal NetIRC.Client Client;
        internal CommandExecutor Executor;

        public MainForm()
        {
            InitializeComponent();

            BackColor = Constants.BACKGROUND_COLOR;
            ForeColor = Constants.TEXT_COLOR;

            MainMenuStrip.BackColor = BackColor;
            MainMenuStrip.ForeColor = ForeColor;

            InputBox.BackColor = Constants.CHAT_BACKGROUND_COLOR;
            InputBox.ForeColor = ForeColor;

            TopicLabel.BackColor = Constants.BACKGROUND_COLOR;
            TopicLabel.ForeColor = Constants.TOPIC_TEXT_COLOR;
            TopicLabel.Text = "";

            Executor = new CommandExecutor();

            Client = new Client();
            Client.Connect("frogbox.es", 6667, false, new User(Properties.Settings.Default.Nickname, Properties.Settings.Default.Color.Substring(1) + "QQ"));
            Client.OnConnect += Client_OnConnect;
            Client.OnChannelJoin += Client_OnChannelJoin;
            Client.OnChannelLeave += Client_OnChannelLeave;
            Client.Server.OnWho += Server_OnWho;
        }

        #region Form Overrides
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Client.Disconnect();

            base.OnClosing(e);

            Application.Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            Logger.Log("[MainForm.OnResize] MainForm original size: " + this.Width + "x" + this.Height);

            base.OnResize(e);

            this.TopicLabel.MaximumSize = new Size(this.ClientRectangle.Width - 6, 0);

            this.Invalidate();

            Logger.Log("MainForm.OnResize was called.");
            Logger.Log("[MainForm.OnResize] MainForm size: " + this.Width + "x" + this.Height);
        }
        #endregion

        #region NetIRC Event Handlers
        #region Client
        void Client_OnConnect(Client client)
        {
            JoinChannels();
            Client.OnConnect -= Client_OnConnect;
        }

        void Client_OnChannelJoin(Client client, Channel channel)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Client.OnChannelJoinHandler(Client_OnChannelJoin), client, channel);
                return;
            }

            ChannelTab tab = new ChannelTab(channel);

            this.AddTab(tab);

            ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + channel.Name], this.Client);
            output.AddJoinLine(channel);

            this.PopulateUserlist();
            this.UpdateTopicLabel();

            this.TabList.SwitchToTab(tab.Name);

            foreach (User user in channel.Users.Values)
            {
                user.OnQuit += user_OnQuit;
                user.OnNickNameChange += user_OnNickNameChange;
                user.OnUserNameChange += user_OnUserNameChange;
            }

            channel.OnMessage += channel_OnMessage;
            channel.OnSendMessage += channel_OnSendMessage;
            channel.OnAction += channel_OnAction;
            channel.OnSendAction += channel_OnSendAction;
            channel.OnNotice += channel_OnNotice;
            channel.OnSendNotice += channel_OnSendNotice;
            channel.OnJoin += channel_OnJoin;
            channel.OnLeave += channel_OnLeave;
            channel.OnKick += channel_OnKick;
            channel.OnTopicChange += channel_OnTopicChange;
            channel.OnWho += channel_OnWho;
        }

        void Client_OnChannelLeave(Client client, Channel channel)
        {
            this.RemoveTab("#" + channel.Name);
            this.PopulateUserlist();
            this.UpdateTopicLabel();

            channel.OnMessage -= channel_OnMessage;
            channel.OnSendMessage -= channel_OnSendMessage;
            channel.OnAction -= channel_OnAction;
            channel.OnSendAction -= channel_OnSendAction;
            channel.OnNotice -= channel_OnNotice;
            channel.OnSendNotice -= channel_OnSendNotice;
            channel.OnJoin -= channel_OnJoin;
            channel.OnLeave -= channel_OnLeave;
            channel.OnKick -= channel_OnKick;
            channel.OnTopicChange -= channel_OnTopicChange;
        }
        #endregion

        #region Server
        void Server_OnWho(Server server, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Server.OnWhoHandler(Server_OnWho), server, message);
            }

            System.Timers.Timer updateTimer = new System.Timers.Timer(1000);

            updateTimer.Elapsed += (s, e) =>
            {
                updateTimer.Enabled = false;

                this.PopulateUserlist();
            };

            updateTimer.Enabled = true;
        }
        #endregion

        #region Channel
        void channel_OnMessage(Channel source, User user, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnMessageHandler(channel_OnMessage), source, user, message);
                return;
            }

            this.TabList.Tabs["#" + source.Name].LineID++;
            ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + source.Name], this.Client);
            output.AddChatLine(user, message);

            if (message.Contains(this.Client.User.NickName) && Properties.Settings.Default.Sounds)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.cs_ping);
                player.PlaySync();
            }
        }

        void channel_OnSendMessage(Channel source, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnSendMessageHandler(channel_OnSendMessage), source, message);
                return;
            }

            this.TabList.Tabs["#" + source.Name].LineID++;
            ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + source.Name], this.Client);
            output.AddChatLine(this.Client.User, message);

            if (message.Contains(this.Client.User.NickName) && Properties.Settings.Default.Sounds)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.cs_ping);
                player.PlaySync();
            }
        }

        void channel_OnAction(Channel source, User user, string action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnActionHandler(channel_OnAction), source, user, action);
                return;
            }

            this.TabList.Tabs["#" + source.Name].LineID++;
            ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + source.Name], this.Client);
            output.AddActionLine(user, action);

            if (action.Contains(this.Client.User.NickName) && Properties.Settings.Default.Sounds)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.cs_ping);
                player.PlaySync();
            }
        }

        void channel_OnSendAction(Channel source, string action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnSendActionHandler(channel_OnSendAction), source, action);
                return;
            }

            this.TabList.Tabs["#" + source.Name].LineID++;
            ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + source.Name], this.Client);
            output.AddActionLine(this.Client.User, action);

            if (action.Contains(this.Client.User.NickName) && Properties.Settings.Default.Sounds)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.cs_ping);
                player.PlaySync();
            }
        }

        void channel_OnNotice(Channel source, User user, string notice)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnNoticeHandler(channel_OnNotice), source, user, notice);
                return;
            }
        }

        void channel_OnSendNotice(Channel source, string notice)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnSendNoticeHandler(channel_OnSendNotice), source, notice);
                return;
            }
        }

        void channel_OnJoin(Channel source, User user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnJoinHandler(channel_OnJoin), source, user);
                return;
            }

            if (user != Client.User)
            {
                user.OnNickNameChange += user_OnNickNameChange;
                user.OnUserNameChange += user_OnUserNameChange;
                user.OnQuit += user_OnQuit;

                ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + source.Name], this.Client);
                output.AddJoinLine(source, user);
            }
        }

        void channel_OnLeave(Channel source, User user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnLeaveHandler(channel_OnLeave), source, user);
                return;
            }

            if (user != Client.User)
            {
                user.OnNickNameChange -= user_OnNickNameChange;
                user.OnQuit -= user_OnQuit;

                ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + source.Name], this.Client);
                output.AddLeaveLine(user, source);
            }
        }

        void channel_OnKick(Channel source, User kicker, User user, string reason)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.Channel.OnKickHandler(channel_OnKick), source, kicker, user, reason);
                return;
            }

            ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + source.Name], this.Client);
            if (user == Client.User)
            {
                output.AddInfoLine(String.Format("You were kicked by {0} ({1}).", kicker.NickName, reason));
                UserList.Nodes.Clear();
            }
            else
            {
                user.OnQuit -= user_OnQuit;
                output.AddInfoLine(String.Format("{0} was kicked by {1} ({2}).", user.NickName, kicker.NickName, reason));
                this.PopulateUserlist();
            }
        }

        void channel_OnTopicChange(Channel source, ChannelTopic topic)
        {
            UpdateTopicLabel();
        }

        void channel_OnWho(Channel source, string message)
        {
            foreach (User user in source.Users.Values)
            {
                user.OnQuit -= user_OnQuit;
                user.OnNickNameChange -= user_OnNickNameChange;
                user.OnUserNameChange -= user_OnUserNameChange;

                user.OnQuit += user_OnQuit;
                user.OnNickNameChange += user_OnNickNameChange;
                user.OnUserNameChange += user_OnUserNameChange;
            }
        }
        #endregion

        #region User
        void user_OnQuit(User user, string reason)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.User.OnQuitHandler(user_OnQuit), user, reason);
                return;
            }

            foreach (Channel channel in user.Channels)
            {
                if (user != Client.User)
                {
                    user.OnNickNameChange -= user_OnNickNameChange;
                    user.OnQuit -= user_OnQuit;

                    ChatOutput output = new ChatOutput(this.TabList.Tabs["#" + channel.Name], this.Client);
                    output.AddQuitLine(user, reason);
                }
            }
        }

        void user_OnNickNameChange(User user, string original)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.User.OnNickNameChangeHandler(user_OnNickNameChange), user, original);
                return;
            }

            foreach (BaseTab tab in TabList.Tabs.Values)
            {
                if (tab.Type == TabType.Channel || tab.Name == original)
                {
                    ChatOutput output = new ChatOutput(tab, this.Client);
                    output.AddInfoLine(String.Format("{0} changed their nickname to {1}.", original, user.NickName));
                }
            }
            this.PopulateUserlist();
        }

        void user_OnUserNameChange(User user, string original)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetIRC.User.OnUserNameChangeHandler(user_OnUserNameChange), user, original);
                return;
            }

            this.PopulateUserlist();
        }
        #endregion
        #endregion

        private void JoinChannels()
        {
            string[] channels = Properties.Settings.Default.Channels.Split(',');

            foreach (string c in channels)
            {
                if (!string.IsNullOrEmpty(c))
                    Client.JoinChannel(c);
            }
        }

        private delegate BaseTab AddTabHandler(BaseTab tab);

        internal BaseTab AddTab(BaseTab tab)
        {
            if (this.InvokeRequired)
            {
                return (BaseTab)this.Invoke(new AddTabHandler(AddTab), tab);
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

        private delegate void RemoveTabHandler(string tabName);

        private void RemoveTab(string tabName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new RemoveTabHandler(RemoveTab), tabName);
                return;
            }

            if (TabList.Tabs.ContainsKey(tabName))
            {
                TabList.RemoveTab(TabList.Tabs[tabName]);
            }
        }

        private delegate void PopulateUserlistHandler();

        private void PopulateUserlist()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new PopulateUserlistHandler(PopulateUserlist));
                return;
            }

            BaseTab selectedTab = TabList.SelectedTab as BaseTab;

            if (selectedTab != null && selectedTab.Type == TabType.Channel)
            {
                ChannelTab channelTab = selectedTab as ChannelTab;

                this.UserList.PopulateFromChannel(channelTab.Channel);
            }

            else
            {
                UserList.Nodes.Clear();
            }
        }

        private delegate void UpdateTopicLabelHandler();

        private void UpdateTopicLabel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateTopicLabelHandler(UpdateTopicLabel));
                return;
            }

            if (TabList.SelectedTab != null && (TabList.SelectedTab as BaseTab).Type == TabType.Channel)
            {
                ChannelTab tab = TabList.SelectedTab as ChannelTab;

                if (tab.Channel.Topic.Message != null && tab.Channel.Topic.Author != null)
                {
                    TopicLabel.Text = tab.Channel.Topic.Message + " (by " + tab.Channel.Topic.Author.NickName + ")";
                }

                else
                {
                    TopicLabel.Text = "";
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
            ChatOutput output = new ChatOutput((this.TabList.SelectedTab as BaseTab), this.Client);
            if (input.StartsWith("/"))
            {
                Executor.Execute(this.Client, input, output);
            }

            else
            {
                if ((TabList.SelectedTab as BaseTab).Type == TabType.Channel)
                {
                    Client.Send(new NetIRC.Messages.Send.ChatMessage((TabList.SelectedTab as ChannelTab).Channel, input));
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
                e.SuppressKeyPress = true;
            }
        }

        private void TopicLabel_ClientSizeChanged(object sender, System.EventArgs e)
        {
            Logger.Log("[TopicLabel.ClientSizeChanged] TopicLabel original size: " + this.TopicLabel.Width + "x" + this.TopicLabel.Height);
            Logger.Log("[TopicLabel.ClientSizeChanged] UserList original size: " + this.UserList.Width + "x" + this.UserList.Height);
            Logger.Log("[TopicLabel.ClientSizeChanged] TabList original size: " + this.TabList.Width + "x" + this.TabList.Height);

            int offset = this.TopicLabel.Bottom + 3;

            this.UserList.Top = offset + 25;
            this.TabList.Top = offset;

            this.TabList.Height = this.Height - this.InputBox.Height - this.TopicLabel.Height - this.MenuStrip.Height - 49;
            this.UserList.Height = this.TabList.Height - 9;

            this.InputBox.Top = this.TabList.Bottom + 3;

            this.TopicLabel.Location = new Point(this.ClientRectangle.Width - this.TopicLabel.Width, this.TopicLabel.Top);

            this.Invalidate();

            Logger.Log("TopicLabel.ClientSizeChanged was handled.");
            Logger.Log("[TopicLabel.ClientSizeChanged] TopicLabel size: " + this.TopicLabel.Width + "x" + this.TopicLabel.Height);
            Logger.Log("[TopicLabel.ClientSizeChanged] UserList size: " + this.UserList.Width + "x" + this.UserList.Height);
            Logger.Log("[TopicLabel.ClientSizeChanged] TabList size: " + this.TabList.Width + "x" + this.TabList.Height);
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

            Logger.Log("MainForm.OnPaint was called.");
        }

        private void OptionsMenuStripItem_Click(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.Show();
        }
        #endregion

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Logger.Log("MainForm.Resize was handled.");

            if (WindowState == FormWindowState.Minimized)
            {
                Logger.Log("[MainForm.Resize] MainForm is minimized.");
            }

            else if (WindowState == FormWindowState.Maximized)
            {
                Logger.Log("[MainForm.Resize] MainForm is maximized.");
            }

            else if (WindowState == FormWindowState.Normal)
            {
                Logger.Log("[MainForm.Resize] MainForm is normal size.");
            }

            Logger.Log("[MainForm.Resize] MainForm size: " + this.Width + "x" + this.Height);
        }

        private void MainForm_Layout(object sender, LayoutEventArgs e)
        {
            Logger.Log("MainForm.Layout was handled.");
            Logger.Log("[MainForm.Layout] MainForm size: " + this.Width + "x" + this.Height);
            Logger.Log("[MainForm.Layout] TopicLabel size: " + this.TopicLabel.Width + "x" + this.TopicLabel.Height);
            Logger.Log("[MainForm.Layout] UserList size: " + this.UserList.Width + "x" + this.UserList.Height);
            Logger.Log("[MainForm.Layout] TabList size: " + this.TabList.Width + "x" + this.TabList.Height);
        }

        private void UserList_Resize(object sender, EventArgs e)
        {
            Logger.Log("[UserList.Resize] TabList original size: " + this.TabList.Width + "x" + this.TabList.Height);
            this.TabList.Width = this.Width - (3 * 7) - this.UserList.Width;
            this.TabList.Height = this.UserList.Height + 9;
            this.InputBox.Width = this.Width - (3 * 10) - this.UserList.Width;
            this.InputBox.Top = this.UserList.Bottom - this.InputBox.Height;
            Logger.Log("UserList.Resize was handled.");
            Logger.Log("[UserList.Resize] TabList size: " + this.TabList.Width + "x" + this.TabList.Height);
        }
    }
}
