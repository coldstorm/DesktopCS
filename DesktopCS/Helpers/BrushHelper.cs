using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DesktopCS.Helpers
{
    public static class BrushHelper
    {
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
