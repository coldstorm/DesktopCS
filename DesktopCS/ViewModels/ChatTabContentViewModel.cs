using System.Windows;
using System.Windows.Documents;
using DesktopCS.Behaviors;

namespace DesktopCS.ViewModels
{
    public class ChatTabContentViewModel
    {
        private readonly FlowDocument _flowDocument = new FlowDocument();

        public FlowDocument FlowDocument
        {
            get { return this._flowDocument; }
        }

        public ChatTabContentViewModel()
        {
            FlowDocumentPagePadding.SetPagePadding(this._flowDocument, new Thickness(0));
        }
    }
}
