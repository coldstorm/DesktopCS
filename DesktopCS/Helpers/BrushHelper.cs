using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace DesktopCS.Helpers
{
    public static class BrushHelper
    {
        public static readonly SolidColorBrush TimeBrush =
            (SolidColorBrush) new BrushConverter().ConvertFrom("#FF373F4E");

        public static readonly SolidColorBrush ChatBrush =
            (SolidColorBrush) new BrushConverter().ConvertFrom("#FFBABBBF");

        public static readonly SolidColorBrush MessageBrush =
            (SolidColorBrush) new BrushConverter().ConvertFrom("#FF808080");

        public static string ToHexWithoutHash(SolidColorBrush brush)
        {
            var c = brush.Color;
            return string.Format("{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
        }

        public static SolidColorBrush  FromHexWithoutHash(string hex)
        {
            try
            {
                return (SolidColorBrush) new BrushConverter().ConvertFrom("#" + hex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to parse brush from hex without hash: " + ex);
                return null;
            }
        }
    }
}
