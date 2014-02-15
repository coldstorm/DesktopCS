using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DesktopCS.Controls;
using DesktopCS.Helpers.Parsers;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services;
using DesktopCS.Services.IRC;

namespace DesktopCS.ViewModels
{
    class MainViewModel : ObservableObject
    {
        private readonly IRCClient _irc;

        public ChatData ChatData { get; private set; }
        public TabManager TabManager { get; private set; }
        public ICommand ChatInputCommand { get; private set; }
        public ICommand QueryCommand { get; private set; }
        public ICommand KickCommand { get; private set; }

        private CSTabItem _selectedItem;

        public CSTabItem SelectedItem
        {
            get { return this._selectedItem; }
            set
            {
                this._selectedItem = value;

                var channelTab = this.TabManager.SelectedTab as ChannelTab;
                if (channelTab != null)
                {
                    this.Users = channelTab.Users;
                    this.Topic = channelTab.Topic;
                }
                else
                {
                    this.Users = null;
                    this.Topic = null;
                }
            }
        }

        private ObservableCollection<UserItem> _users;

        public ObservableCollection<UserItem> Users
        {
            get { return this._users; }
            private set
            {
                this._users = value;
                this.OnPropertyChanged("Users");
            }
        }

        private Topic _topic;

        public Topic Topic
        {
            get { return this._topic; }
            private set
            {
                this._topic = value;
                this.OnPropertyChanged("Topic");
            }
        }

        public MainViewModel()
        {
            this.TabManager = new TabManager();

            LoginData loginData = SettingsManager.Value.GetLoginData();
            IRCSettings ircSettings = SettingsManager.Value.GetIRCSettings();
            this._irc = new IRCClient(this.TabManager, loginData, ircSettings);

            this.ChatData = new ChatData();
            this.ChatInputCommand = new RelayCommand(param => this.Chat(), param => this.CanChat);
            this.QueryCommand = new RelayCommand(param => this.Query((string)param));
            this.KickCommand = new RelayCommand(param => this.Kick((string)param));
        }

        public bool CanChat
        {
            get { return this.ChatData.IsValid; }
        }

        public void Chat()
        {
            string chat = this.ChatData.InputText;
            chat = InputHelper.Parse(chat);

            foreach (var line in chat.Split(Environment.NewLine.ToCharArray()))
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    this._irc.Chat(line);
                }
            }
            
            this.ChatData.InputText = String.Empty;
        }

        public void Query(string nick)
        {
            this._irc.Query(nick);
        }

        public void Kick(string nick)
        {
            this._irc.Kick(nick);
        }
    }
}
