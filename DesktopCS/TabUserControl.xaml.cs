using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DesktopCS
{
    /// <summary>
    /// Interaction logic for TabUserControl.xaml
    /// </summary>
    public partial class TabUserControl
    {
        public TabUserControl()
        {
            InitializeComponent();
        }

        private string CreateTimeStamp()
        {
            return DateTime.Now.ToString("[HH:mm]");
        }

        public void AddChat(Brush color, string username, string chat)
        {
            var p = new Paragraph();
            var usernameRun = new Run(username) { Foreground = color };
            var timeRun = new Run(CreateTimeStamp()) { Foreground = (Brush)TryFindResource("TimeBrush") };
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
