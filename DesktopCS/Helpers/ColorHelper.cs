using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace DesktopCS.Helpers
{
    public static class ColorHelper
    {
        public static readonly Color TimeColor = FromString("#FF373F4E");
        public static readonly Color ChatColor = FromString("#FFBABBBF");
        public static readonly Color MessageColor = FromString("#FF808080");

        public static string ToHexWithoutHash(Color color)
        {
            var c = color;
            return string.Format("{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
        }

        public static Color FromHexWithoutHash(string hex)
        {
            try
            {
                var convertFromString = ColorConverter.ConvertFromString("#" + hex);
                if (convertFromString != null)
                    return (Color) convertFromString;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to parse color from hex without hash: " + ex);
            }

            return default(Color);
        }

        public static Color FromString(string hex)
        {
            var color = ColorConverter.ConvertFromString(hex);
            if (color != null)
                return (Color)color;

            return default(Color);
        }

        public static string ToString(Color color)
        {
            return color.ToString();
        }
    }
}
