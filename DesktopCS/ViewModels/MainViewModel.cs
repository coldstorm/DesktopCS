using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DesktopCS.Controls;
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

        private CSTabItem _selectedItem;

        public CSTabItem SelectedItem
        {
            get { return this._selectedItem; }
            set
            {
                this._selectedItem = value;

                var channelTab = this.TabManager.SelectedTab as ChannelTab;
                this.Users = channelTab != null ? channelTab.Users : null;
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

        public MainViewModel(LoginData loginData)
        {
            this.TabManager = new TabManager();

            this._irc = new IRCClient(this.TabManager, loginData);

            this.ChatData = new ChatData();
            this.ChatInputCommand = new RelayCommand(param => Chat(), param => CanChat);
        }

        public bool CanChat
        {
            get { return this.ChatData.IsValid; }
        }

        public void Chat()
        {
            this._irc.Chat(this.ChatData.InputText);
            this.ChatData.InputText = String.Empty;
        }
    }
}
