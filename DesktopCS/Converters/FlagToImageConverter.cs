using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using DesktopCS.Helpers;

namespace DesktopCS.Converters
{
    class FlagToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (string)value;
            var uri = String.Format("Resources/Flags/{0}.png", flag);

            // BUG: This is taking too much processing time
            if (!ResourceHelper.ResourceExists(uri))
            {
                uri = "Resources/Flags/QQ.png"; 
            }
            var bitmapImage = new BitmapImage(new Uri("pack://application:,,,/" + uri));
            var img = new Image { Source = bitmapImage, };
            bitmapImage.Freeze();
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
