using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using DesktopCS.Helpers.Extensions;

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
                    if (args.SoundNotification)
                        SoundHelper.PlaySound(SoundHelper.PingUri);
                    
                    // Flash the window indefinitely if desktop notifications are enabled
                    // TODO: Add setting for flashing window X number of times
                    if (args.DesktopNotification)
                        WindowExtensions.FlashWindow(Application.Current.MainWindow);

                    return new Run(s)
                    {
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(ColorHelper.HighlightColor)
                    };
                });
        }
    }
}