using System;
using System.Windows;
using DesktopCS.Controls;
using DesktopCS.ViewModels;
using DesktopCS.Views;

namespace DesktopCS.Tabs
{
    public class Tab : UIInvoker
    {
        private string _header;
        public string Header
        {
            get { return _header; }
            set
            {
                var original = value;
                _header = value;
                TabItem.Header = value;
                OnHeaderChange(original);
            }
        }

        public ChatTabContentView TabView { get; private set; }
        public ChatTabContentViewModel TabVM { get; private set; }
        public CSTabItem TabItem { get; private set; }

        public delegate void HeaderChangeHandler(object sender, string oldValue);
        public event HeaderChangeHandler HeaderChange;

        protected virtual void OnHeaderChange(string oldvalue)
        {
            HeaderChangeHandler handler = HeaderChange;
            if (handler != null) handler(this, oldvalue);
        }

        public event EventHandler<EventArgs> Close;

        protected virtual void OnClose()
        {
            EventHandler<EventArgs> handler = Close;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public Tab(string header)
        {
            TabVM = new ChatTabContentViewModel();
            TabView = new ChatTabContentView(TabVM);
            TabItem = new CSTabItem {Content = TabView};
            Header = header;

            TabItem.CloseTab += TabItem_CloseTab;
        }

        void TabItem_CloseTab(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            OnClose();
        }

        public void AddChat(ChatLine line, bool markUnread = true)
        {
            if (InvokeRequired)
            {
                Invoke(() => AddChat(line, markUnread));
                return;
            }

            if (markUnread)
                MarkUnread();

            TabVM.FlowDocument.Blocks.Add(line.ToParagraph());
        }

        public void MarkUnread()
        {
            if (InvokeRequired)
            {
                Invoke(MarkUnread);
                return;
            }

            if (!TabItem.IsSelected)
                TabItem.IsUnread = true;
        }
    }
}