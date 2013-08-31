using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Controls;
using DesktopCS.Helpers;
using DesktopCS.ViewModels;
using DesktopCS.Views;

namespace DesktopCS.Tabs
{
    public class Tab
    {
        private static readonly Brush _timeBrush = (Brush)Application.Current.FindResource("TimeBrush");
        private static readonly Brush _chatBrush = (Brush)Application.Current.FindResource("ChatBrush");

        public ChatTabContentView TabView { get; private set; }
        public ChatTabContentViewModel TabVM { get; private set; }
        public CSTabItem TabItem { get; private set; }

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
            TabItem = new CSTabItem {Header = header, Content = TabView};

            TabItem.CloseTab += TabItem_CloseTab;
        }

        void TabItem_CloseTab(object sender, RoutedEventArgs e)
        {
            OnClose();

        }

        public void AddChat(ChatLine line, bool markUnread = true)
        {
            if (markUnread)
                MarkUnread();

            var p = new Paragraph();

            if (!String.IsNullOrEmpty(line.Timestamp))
            {
                var timeRun = new Run(line.Timestamp) { Foreground = _timeBrush };
                p.Inlines.Add(timeRun);
                p.Inlines.Add(" ");
            }

            if (line.User != null)
            {
                Brush brush = _chatBrush;
                if (line.User.Metadata != null)
                    brush = line.User.Metadata.Color;

                var usernameRun = new Run(line.User.Nickname) { Foreground = brush };
                p.Inlines.Add(usernameRun);
                p.Inlines.Add(" ");
            }

            var chatRun = new Run(line.Chat) { Foreground = line.ChatBrush };
            p.Inlines.Add(chatRun);

            TabVM.FlowDocument.Blocks.Add(p);
        }

        public virtual void MarkUnread()
        {
            if (!TabItem.IsSelected)
                TabItem.IsUnread = true;
        }
    }
}