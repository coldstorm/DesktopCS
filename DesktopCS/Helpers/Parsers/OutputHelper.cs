using System;
using System.Windows.Documents;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    class OutputHelper
    {
        public static Span Parse(string text, ParseArgs args)
        {
            return InputHelper.Parse(text, args,
                text2 => MIRCHelper.Parse(text2, args,
                    text3 => URLHelper.Parse(text3, args,
                        text4 => HighlightHelper.Parse(text4, args,
                            text5 => new Run(text5)))));
        }
    }
}
