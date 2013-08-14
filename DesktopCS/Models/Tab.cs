using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Controls;
using DesktopCS.Helpers;
using DesktopCS.UserControls;
using DesktopCS.ViewModels;

namespace DesktopCS.Models
{
    public class Tab
    {
        private readonly TabUserControlViewModel _tab;
        private readonly CSTabItem _parent;
        private static readonly Brush _timeBrush = (Brush) Application.Current.FindResource("TimeBrush");

        public Tab(TabUserControlViewModel tab, CSTabItem parent)
        {
            _tab = tab;
            _parent = parent;
        }

        public void AddChat(ChatLine line, bool dispatch = false)
        {
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
                var usernameRun = new Run(line.User.Nickname) { Foreground = line.User.Metadata.Color };
                p.Inlines.Add(usernameRun);
                p.Inlines.Add(" ");
            }

            var chatRun = new Run(line.Chat) { Foreground = line.ChatBrush };
            p.Inlines.Add(chatRun);

            _tab.FlowDocument.Blocks.Add(p);
        }

        public void MarkUnread()
        {
            if (!_parent.IsSelected)
                _parent.IsUnread = true;
        }
    }
}
