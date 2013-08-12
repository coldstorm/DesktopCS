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

        public void AddChat(Brush color, string username, string chat)
        {
            if (!_parent.IsSelected)
                _parent.IsUnread = true;

            var p = new Paragraph();
            var usernameRun = new Run(username) { Foreground = color };
            var timeRun = new Run(TimeHelper.CreateTimeStamp()) { Foreground = (Brush)TryFindResource("TimeBrush") };
            var chatRun = new Run(chat) {Foreground = (Brush) FindResource("ChatBrush") };
            p.Inlines.Add(timeRun);
            p.Inlines.Add(" ");
            p.Inlines.Add(usernameRun);
            p.Inlines.Add(" ");
            p.Inlines.Add(chatRun);

            FlowDoc.Blocks.Add(p);
        }
    }
}
