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
            Regex highlightRegex = new Regex(args.HostNickname, RegexOptions.ExplicitCapture);

            var span = new Span();

            int lastPos = 0;
            foreach (Match match in highlightRegex.Matches(text))
            {
                if (match.Index != lastPos)
                {
                    var rawText = text.Substring(lastPos, match.Index - lastPos);
                    Inline inline = callback(rawText);
                    span.Inlines.Add(inline);
                }

                var highlight = new Run(match.Value)
                {
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(ColorHelper.HighlightColor)
                };

                span.Inlines.Add(highlight);

                lastPos = match.Index + match.Length;
            }

            if (lastPos < text.Length)
            {
                Inline inline = callback(text.Substring(lastPos));
                span.Inlines.Add(inline);
            }

            return span;
        }
    }
}
