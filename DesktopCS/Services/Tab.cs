using System;
using System.Windows;
using DesktopCS.Controls;
using DesktopCS.Models;
using DesktopCS.ViewModels;
using DesktopCS.Views;

namespace DesktopCS.Services
{
    public class Tab : UIInvoker
    {
        private string _header;
        public string Header
        {
            get { return this._header; }
            set
            {
                var original = value;
                this._header = value;
                this.TabItem.Header = value;
                this.OnHeaderChange(original);
            }
        }

        public ChatTabContentView TabView { get; private set; }
        public ChatTabContentViewModel TabVM { get; private set; }
        public CSTabItem TabItem { get; private set; }

        public delegate void HeaderChangeHandler(object sender, string oldValue);
        public event HeaderChangeHandler HeaderChange;

        protected virtual void OnHeaderChange(string oldvalue)
        {
            HeaderChangeHandler handler = this.HeaderChange;
            if (handler != null) handler(this, oldvalue);
        }

        public event EventHandler<EventArgs> Close;

        protected virtual void OnClose()
        {
            EventHandler<EventArgs> handler = this.Close;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public Tab(string header)
        {
            this.TabVM = new ChatTabContentViewModel();
            this.TabView = new ChatTabContentView(this.TabVM);
            this.TabItem = new CSTabItem {Content = this.TabView};
            this.Header = header;

            this.TabItem.CloseTab += this.TabItem_CloseTab;
        }

        void TabItem_CloseTab(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            this.OnClose();
        }

        public void AddChat(ChatLine line, bool markUnread = true)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => this.AddChat(line, markUnread));
                return;
            }

            if (markUnread)
                this.MarkUnread();

            this.TabVM.FlowDocument.Blocks.Add(line.ToParagraph());
        }

        public void MarkUnread()
        {
            if (!this.TabItem.IsSelected)
                this.TabItem.IsUnread = true;
        }
    }
}