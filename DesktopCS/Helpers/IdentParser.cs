using System;
using System.Text.RegularExpressions;
using System.Windows.Media;
using DesktopCS.Models;

namespace DesktopCS.Helpers
{
    public static class IdentParser
    {
        public static UserMetadata Parse(string ident)
        {
            var regexp = new Regex(@"^([0-9a-f]{3}|[0-9a-f]{6})([a-z]{2})$", RegexOptions.IgnoreCase);
            Match match = regexp.Match(ident);

            if (match.Success)
            {
                var color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + match.Groups[1].Value));
                var flag = match.Groups[2].Value;
                return new UserMetadata(color, flag);
            }

            throw new ArgumentException("Identity could not be parsed.", "ident");
        }
    }
}
