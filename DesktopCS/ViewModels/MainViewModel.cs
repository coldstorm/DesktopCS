using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using DesktopCS.Controls;

namespace DesktopCS.ViewModels
{
    class MainViewModel
    {
        public MainViewModel()
        {
            _tabManager = new TabManager(Tabs);

            _tabManager["#Coldstorm"].AddChat(Brushes.White, "Processor", "Test");
            _tabManager.MarkUnread("#Coldstorm");
        }

        private readonly ObservableCollection<CSTabItem> _tabs = new ObservableCollection<CSTabItem>();

        public ObservableCollection<CSTabItem> Tabs
        {
            get { return _tabs; }
        }

        private readonly TabManager _tabManager;

        private int _selectedTabIndex;

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                Tabs[value].Unread = false;
            }
        }
    }
}
