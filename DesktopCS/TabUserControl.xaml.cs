using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopCS
{
    /// <summary>
    /// Interaction logic for TabUserControl.xaml
    /// </summary>
    public partial class TabUserControl : UserControl
    {
        public TabUserControl()
        {
            InitializeComponent();
        }

        public void AddChat(Brush color, string username, string chat)
        {
            var p = new Paragraph();
            var dateRun = new Run("[00:00]") {Foreground = (Brush) FindResource("TimeBrush")};
            var usernameRun = new Run(username) { Foreground = color };
            var chatRun = new Run(chat) {Foreground = (Brush) FindResource("ChatBrush") };
            p.Inlines.Add(dateRun);
            p.Inlines.Add(" ");
            p.Inlines.Add(usernameRun);
            p.Inlines.Add(" ");
            p.Inlines.Add(chatRun);

            FlowDoc.Blocks.Add(p);
        }
    }
}
