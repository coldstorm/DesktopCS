using System.Windows.Documents;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    class OutputHelper
    {
        public static Span Parse(string text, Color foreground, ParseArgs args)
        {
            var newText = InputHelper.Parse(text);
            var span = new Span();

            span = HighlightHelper.Parse(newText, args);
            span = URLHelper.Parse(newText, args);
            span = MIRCHelper.Parse(newText, foreground, null);

            return span;
        }
    }
}
