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

namespace DesktopCS.Controls
{
    public class UserlistItem : ContentControl
    {
        static UserlistItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserlistItem), new FrameworkPropertyMetadata(typeof(UserlistItem)));
        }

        public static DependencyProperty FlagProperty = DependencyProperty.Register("Flag", typeof(Image), typeof(UserlistItem));

        [System.ComponentModel.Description("Flag")]
        [System.ComponentModel.Category("Common")]
        public Image Flag
        {
            get { return (Image)GetValue(FlagProperty); }
            set { SetValue(FlagProperty, value); }
        }
    }
}
