using System.Windows;
using System.Windows.Controls;

namespace DesktopCS.Controls
{
    public class UserlistItem : HeaderedContentControl
    {
        static UserlistItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserlistItem), new FrameworkPropertyMetadata(typeof(UserlistItem)));
        }

        public static DependencyProperty FlagProperty = DependencyProperty.Register("Flag", typeof(Image), typeof(UserlistItem));

        public Image Flag
        {
            get { return (Image)this.GetValue(FlagProperty); }
            set { this.SetValue(FlagProperty, value); }
        }


        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(UserlistItem));

        public bool IsExpanded
        {
            get { return (bool)this.GetValue(IsExpandedProperty); }
            set { this.SetValue(IsExpandedProperty, value); }
        }
    }
}
