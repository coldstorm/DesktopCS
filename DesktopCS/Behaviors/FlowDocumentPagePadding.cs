using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;

namespace DesktopCS.Behaviors
{
    public static class FlowDocumentPagePadding
    {
        public static Thickness GetPagePadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(PagePaddingProperty);
        }

        public static void SetPagePadding(DependencyObject obj, Thickness value)
        {
            obj.SetValue(PagePaddingProperty, value);
        }

        public static readonly DependencyProperty PagePaddingProperty =
            DependencyProperty.RegisterAttached("PagePadding", typeof(Thickness), typeof(FlowDocumentPagePadding), new UIPropertyMetadata(new Thickness(double.NegativeInfinity), (o, args) =>
            {
                var fd = o as FlowDocument;
                if (fd == null) return;
                var descriptor = DependencyPropertyDescriptor.FromProperty(FlowDocument.PagePaddingProperty, typeof(FlowDocument));
                descriptor.RemoveValueChanged(fd, PaddingChanged);
                fd.PagePadding = (Thickness)args.NewValue;
                descriptor.AddValueChanged(fd, PaddingChanged);
            }));

        public static void PaddingChanged(object s, EventArgs e)
        {
            ((FlowDocument)s).PagePadding = GetPagePadding((DependencyObject)s);
        }
    }
}
