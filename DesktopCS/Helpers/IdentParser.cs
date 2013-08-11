using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace DesktopCS.Helpers
{
    public static class IdentParser
    {
        public static bool Parse(string ident, out SolidColorBrush color, out string flag)
        {
            Regex regexp = new Regex(@"^([0-9a-f]{3}|[0-9a-f]{6})([a-z]{2})$", RegexOptions.IgnoreCase);
            Match match = regexp.Match(ident);

            if (match.Success)
            {
                color = (SolidColorBrush)(new BrushConverter().ConvertFrom(match.Groups[0].Value));
                flag = match.Groups[1].Value;
                return true;
            }

            color = null;
            flag = null;
            return false;
        }
    }
}
