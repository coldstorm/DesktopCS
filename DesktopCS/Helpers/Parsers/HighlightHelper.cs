using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Services.IRC;

namespace DesktopCS.Helpers.Parsers
{
    public static class HighlightHelper
    {
        public static Inline Parse(string text, ParseArgs args)
        {
            Regex _highlightRegex = new Regex(args.HostNickname, RegexOptions.ExplicitCapture);

            var span = new Span();

            int lastPos = 0;
            foreach (Match match in _highlightRegex.Matches(text))
            {
                if (match.Index != lastPos)
                {
                    var rawText = text.Substring(lastPos, match.Index - lastPos);
                    span.Inlines.Add(rawText);
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
                span.Inlines.Add(new Run(text.Substring(lastPos)));

            return span;
        }
    }
}
