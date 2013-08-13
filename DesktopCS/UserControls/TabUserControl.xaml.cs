using System;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Controls;
using DesktopCS.Helpers;

namespace DesktopCS.UserControls
{
    /// <summary>
    /// Interaction logic for TabUserControl.xaml
    /// </summary>
    public partial class TabUserControl
    {
        private readonly CSTabItem _parent;

        public TabUserControl(CSTabItem parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        public void AddSystemMessage(string chat)
        {
            AddChat((Brush) FindResource("MessageBrush"), chat);
        }

        public void AddChat(Brush chatBrush, string chat)
        {
            AddChat(null, null, chatBrush, chat);
        }

        public void AddChat(Brush usernameBrush, string username, Brush chatBrush, string chat)
        {
            AddChat((Brush) FindResource("TimeBrush"), TimeHelper.CreateTimeStamp(), usernameBrush, username, chatBrush, chat);
        }

        public void AddChat(Brush timeBrush, string time, Brush usernameBrush, string username, Brush chatBrush, string chat)
        {
            if (!_parent.IsSelected)
                _parent.IsUnread = true;

            var p = new Paragraph();

            if (!String.IsNullOrEmpty(time))
            {
                var timeRun = new Run(time) {Foreground = timeBrush};
                p.Inlines.Add(timeRun);
                p.Inlines.Add(" ");
            }

            if (!String.IsNullOrEmpty(username))
            {
                var usernameRun = new Run(username) {Foreground = usernameBrush};
                p.Inlines.Add(usernameRun);
                p.Inlines.Add(" ");
            }

            var chatRun = new Run(chat) { Foreground = chatBrush };
            p.Inlines.Add(chatRun);

            FlowDoc.Blocks.Add(p);
        }
    }
}
