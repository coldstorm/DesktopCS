using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using DesktopCS.ViewModels;
using System.Windows.Controls;

namespace DesktopCS.Views
{
    /// <summary>
    /// Interaction logic for TabUserControl.xaml
    /// </summary>
    public partial class ChatTabContentView
    {
        public TextRange Selection
        {
            get { return this.ChatRichTextBox.Selection; }
        }

        public ChatTabContentView(ChatTabContentViewModel vm)
        {
            this.InitializeComponent();
            this.DataContext = vm;
            this.SetBinding(DocumentProperty, new Binding("FlowDocument"));
        }

        public FlowDocument Document
        {
            get { return (FlowDocument)this.GetValue(DocumentProperty); }
            set { this.SetValue(DocumentProperty, value); }
        }

        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument), typeof(ChatTabContentView), new PropertyMetadata(OnDocumentChanged));

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ChatTabContentView)d;
            if (e.NewValue == null)
                control.ChatRichTextBox.Document = new FlowDocument(); //Document is not amused by null :)

            control.ChatRichTextBox.Document = control.Document;
        }

        public void ScrollToEnd()
        {
            this.ChatScrollView.ScrollToEnd();
        }
    }
}
