using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;

namespace DesktopCS.Helpers.Parsers
{
    public static class HighlightHelper
    {
        public static Span Parse(string text, ParseArgs args, Func<string, Inline> callback)
        {
            var highlightRegex = new Regex(String.Format(@"\b{0}\b", args.HostNickname), RegexOptions.IgnoreCase);

            return RegexHelper.Parse(text, highlightRegex, args, callback,
                s =>
                {
                    // Play the ping sound if it is enabled
                    if (args.PingSound)
                        SoundHelper.PlaySound(SoundHelper.PingUri);

                    return new Run(s)
                    {
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(ColorHelper.HighlightColor)
                    };
                });
        }
    }
}