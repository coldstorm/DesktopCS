using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.Controls;
using DesktopCS.UserControls;

namespace DesktopCS
{
    public class TabManager
    {
        private readonly ObservableCollection<CSTabItem> _tabs;

        public TabManager(ObservableCollection<CSTabItem> tabs)
        {
            _tabs = tabs;
        }

        public TabUserControl this[string tabName]
        {
            get
            {
                var firstOrDefault = _tabs.FirstOrDefault(t => (string) t.Header == tabName);
                if (firstOrDefault != null)
                    return (TabUserControl) firstOrDefault.Content;

                var tabContent = new TabUserControl();
                var tab = new CSTabItem {Header = tabName, Content = tabContent};
                _tabs.Add(tab);
                return tabContent;
            }
        }

        public void MarkUnread(string tabName)
        {
            var firstOrDefault = _tabs.FirstOrDefault(t => (string)t.Header == tabName);
            if (firstOrDefault != null && firstOrDefault.IsSelected == false)
                firstOrDefault.Unread = true;
        }
    }
}
