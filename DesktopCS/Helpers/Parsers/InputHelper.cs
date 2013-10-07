using System;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace DesktopCS.Helpers.Parsers
{
    public static class InputHelper 
    {
        public static Span Parse(string text, ParseArgs args, Func<string, Inline> callback)
        {
            string newText = Parse(text);
            Inline inline = callback(newText);
            return new Span(inline);
        }

        public static string Parse(string text)
        {
            // Spoiler
            var regexp = new Regex(@"::(?<spoiler>.+?)::", RegexOptions.ExplicitCapture);
            text = regexp.Replace(text, MIRCHelper.ColorChar + "1,1${spoiler}" + MIRCHelper.ColorChar);

            // Bold
            text = text.Replace(@"\b", new String(MIRCHelper.BoldChar, 1));

            // Italics
            text = text.Replace(@"\i", new String(MIRCHelper.ItalicChar, 1));
            text = text.Replace(@"\s", new String(MIRCHelper.ItalicChar, 1));

            // Underline
            text = text.Replace(@"\u", new String(MIRCHelper.UnderlineChar, 1));

            // Reset
            text = text.Replace(@"\o", new String(MIRCHelper.ResetChar, 1));

            //Reverse
            text = text.Replace(@"\r", new String(MIRCHelper.ReverseChar, 1));

            // Color
            text = text.Replace(@"\c", new String(MIRCHelper.ColorChar, 1));

            return text;
        }
    }
}
