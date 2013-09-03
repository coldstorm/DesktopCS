using System.Windows;
using System.Windows.Documents;
using DesktopCS.Behaviors;

namespace DesktopCS.ViewModels
{
    public class ChatTabContentViewModel
    {
        public ChatTabContentViewModel()
        {
            FlowDocumentPagePadding.SetPagePadding(this._flowDocument, new Thickness(0));
        }

        private readonly FlowDocument _flowDocument = new FlowDocument();

        public FlowDocument FlowDocument
        {
            get { return this._flowDocument; }
        }
    }
}
