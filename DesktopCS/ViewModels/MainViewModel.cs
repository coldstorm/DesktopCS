using System.Collections.ObjectModel;
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
            _tabManager["#2"].AddChat(Brushes.White, "Processor", "Test");
        }

        private readonly ObservableCollection<CSTabItem> _tabs = new ObservableCollection<CSTabItem>();

        public ObservableCollection<CSTabItem> Tabs
        {
            get { return _tabs; }
        }

        private readonly TabManager _tabManager;

    }
}
