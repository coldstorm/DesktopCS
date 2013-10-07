using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    public static class HighlightHelper
    {
        public static Span Parse(string text, ParseArgs args, Func<string, Inline> callback)
        {
            var highlightRegex = new Regex(args.HostNickname, RegexOptions.ExplicitCapture);

            return RegexHelper.Parse(text, highlightRegex, args, callback,
                s => new Run(s)
                {
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(ColorHelper.HighlightColor)
                });
        }
    }
}
