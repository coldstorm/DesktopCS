using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace DesktopCS.Helpers.Parsers
{
    public static class ImageHelper
    {
        public static Span Parse(string text, ParseArgs args, Func<string, Inline> callback)
        {
            return RegexHelper.Parse(text, _imageRegex, args, callback,
                s =>
                {
                    BitmapImage bi = new BitmapImage(new Uri(s));
                    Image image = new Image();
                    image.Source = bi;
                    InlineUIContainer container = new InlineUIContainer(image);
                    return container;
                });
        }

        private static readonly Regex _imageRegex = new Regex(@"(.+?).(png|tif|jpg|jpeg|bmp|gif)", RegexOptions.IgnoreCase);
    }
}
