using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    public static class RegexHelper
    {
        public static Span Parse(string text, Regex regex, ParseArgs args, Func<string, Inline> noMatchCallback, Func<string, Inline> matchCallback)
        {
            var span = new Span();

            int lastPos = 0;
            foreach (Match match in regex.Matches(text))
            {
                if (match.Index != lastPos)
                {
                    var rawText = text.Substring(lastPos, match.Index - lastPos);
                    Inline inline = noMatchCallback(rawText);
                    span.Inlines.Add(inline);
                }


                Inline matchInline = matchCallback(match.Value);
                span.Inlines.Add(matchInline);

                lastPos = match.Index + match.Length;
            }

            if (lastPos < text.Length)
            {
                Inline inline = noMatchCallback(text.Substring(lastPos));
                span.Inlines.Add(inline);
            }

            return span;
        }
    }
}
