using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Media;
using DesktopCS.Models;

namespace DesktopCS.Helpers
{
    public static class IdentParser
    {
        public static UserMetadata Parse(string ident)
        {
            var regexp = new Regex(@"^(?:[0-9a-f]{3}){1,2}|[a-z]{2}$", RegexOptions.IgnoreCase);
            MatchCollection match = regexp.Matches(ident);

            if (match.Count == 2)
            {
                var color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + match[0].Value));
                var flag = match[1].Value;
                return new UserMetadata(color, flag);
            }

            throw new ArgumentException("Identity could not be parsed.", "ident");
        }
    }
}
