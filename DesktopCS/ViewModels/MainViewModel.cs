using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using DesktopCS.Commands;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.Services;
using DesktopCS.Services.IRC;

namespace DesktopCS.ViewModels
{
    class MainViewModel 
    {
        private readonly TabManager _tabManager;
        private readonly IRCClient _irc;

        public ChatData ChatData { get; private set; }
        public CompositeCollection Tabs { get; private set; }
        public ChatInputCommand ChatInputCommand { get; private set; }

        public MainViewModel(SettingsManager settingsManager, LoginData loginData)
        {
            this._tabManager = new TabManager();
            this.Tabs = this._tabManager.Tabs;

            this._irc = new IRCClient(this._tabManager, loginData);

            this.ChatData = new ChatData();
            this.ChatInputCommand = new ChatInputCommand(this);
        }

        public bool CanChat
        {
            get { return this.ChatData.IsValid; }
        }

        public void Chat()
        {
            this._irc.Chat(ChatData.InputText);
            this.ChatData.InputText = String.Empty;
        }
    }
}
