using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using DesktopCS.Commands;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services;
using DesktopCS.Services.IRC;

namespace DesktopCS.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private readonly IRCClient _irc;
        private CSTabItem _selectedItem;
        private ObservableCollection<UserItem> _users;

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

        public ObservableCollection<UserItem> Users
        {
            get { return this._users; }
            private set
            {
                this._users = value;
                this.OnPropertyChanged("Users");
            }
        }

        public ChatData ChatData { get; private set; }
        public ChatInputCommand ChatInputCommand { get; private set; }

        public TabManager TabManager { get; private set; }

        public MainViewModel(SettingsManager settingsManager, LoginData loginData)
        {
            this.TabManager = new TabManager();

            this._irc = new IRCClient(this.TabManager, loginData);

            this.ChatData = new ChatData();
            this.ChatInputCommand = new ChatInputCommand(this);
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
