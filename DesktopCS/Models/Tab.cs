using System;
using System.Windows;
using System.Windows.Documents;
using DesktopCS.Controls;
using DesktopCS.Helpers.Extensions;

namespace DesktopCS.Models
{
    public class Tab
    {
        private readonly FlowDocument _flowDoc;

        public CSTabItem TabItem { get; private set; }

        public string Header
        {
            get { return (string)this.TabItem.Header; }
            set
            {
                var original = (string)this.TabItem.Header;
                if (original == value) return;

                this.TabItem.Header = value;
                this.OnHeaderChange(original);
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.TabItem.IsSelected;
            }
            set
            {
                this.TabItem.IsSelected = value;
            }
        }

        public bool IsClosable {
            get
            {
                return this.TabItem.IsClosable;
            }
            set
            {
                this.TabItem.IsClosable = value;
            }
        }

        public delegate void HeaderChangeHandler(object sender, string oldValue);
        public event HeaderChangeHandler HeaderChange;

        protected virtual void OnHeaderChange(string oldvalue)
        {
            HeaderChangeHandler handler = this.HeaderChange;
            if (handler != null) handler(this, oldvalue);
        }

        public event EventHandler<EventArgs> Part;

        protected virtual void OnPart()
        {
            EventHandler<EventArgs> handler = this.Part;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> Close;

        protected virtual void OnClose()
        {
            EventHandler<EventArgs> handler = this.Close;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public Tab(string header, FlowDocument flowDoc, CSTabItem tabItem)
        {
            this._flowDoc = flowDoc;
            this.TabItem = tabItem;
            this.Header = header;

            this.TabItem.CloseTab += this.TabItem_CloseTab;
            this.TabItem.PartTab += this.TabItem_PartTab;
        }

        void TabItem_PartTab(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            this.OnPart();
        }

        void TabItem_CloseTab(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            this.OnClose();
        }

        public void AddChat(ChatLine line)
        {
            if (line is MessageLine)
                this.MarkUnread();

            this._flowDoc.Blocks.Add(line.ToParagraph());
        }

        public virtual void MarkUnread()
        {
            if (!this.IsSelected)
                this.TabItem.IsUnread = true;
        }
    }
}