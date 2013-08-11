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
        private ObservableCollection<CSTabItem> _tabs;

        public TabManager(ObservableCollection<CSTabItem> tabs)
        {
            this._tabs = tabs;
        }

        public TabUserControl this[string tabName]
        {
            get
            {
                return (TabUserControl)_tabs.FirstOrDefault(t => (string)t.Header == tabName).Content;
            }
        }

        public void MarkUnread(string tabName)
        {
            _tabs.FirstOrDefault(t => (string)t.Header == tabName).Unread = true;
        }
    }
}
