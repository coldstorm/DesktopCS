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
    public class Channel
    {
        private readonly TabUserControlViewModel _tab;
        private readonly CSTabItem _parent;

        public Channel(TabUserControlViewModel tab, CSTabItem parent)
        {
            _tab = tab;
            _parent = parent;
        }

        public void AddChat(ChatLine line)
        {
            MarkUnread();

            var p = new Paragraph();

            if (!String.IsNullOrEmpty(line.Timestamp))
            {
                var timeRun = new Run(line.Timestamp) { Foreground = (Brush) Application.Current.FindResource("TimeBrush") };
                p.Inlines.Add(timeRun);
                p.Inlines.Add(" ");
            }

            if (line.User != null)
            {
                var usernameRun = new Run(line.User.Username) { Foreground = line.UserBrush };
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
