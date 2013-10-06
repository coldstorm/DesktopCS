using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Win32;

namespace DesktopCS.Helpers.Parsers
{
    public static class URLHelper
    {
        private static readonly Regex _urlRegex = new Regex(@"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?");

        public static Span Parse(string text, ParseArgs args)
        {
            var span = new Span();

            // Find all URLs using a regular expression
            int lastPos = 0;
            foreach (Match match in _urlRegex.Matches(text))
            {
                // Copy raw string from the last position up to the match
                if (match.Index != lastPos)
                {
                    var rawText = text.Substring(lastPos, match.Index - lastPos);
                    span.Inlines.Add(new Run(rawText));
                }

                // Create a hyperlink for the match
                var link = new Hyperlink(new Run(match.Value))
                {
                    NavigateUri = new Uri(match.Value),
                    Foreground = new SolidColorBrush(ColorHelper.HyperlinkColor)
                };
                link.Click += OnUrlClick;

                span.Inlines.Add(link);

                // Update the last matched position
                lastPos = match.Index + match.Length;
            }

            // Finally, copy the remainder of the string
            if (lastPos < text.Length)
                span.Inlines.Add(new Run(text.Substring(lastPos)));

            return span;
        }

        private static void OnUrlClick(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;

            string browserPath = GetBrowserPath();
            if (browserPath == string.Empty)
                browserPath = "iexplore";

            var process = new Process {StartInfo = new ProcessStartInfo(browserPath)};
            process.StartInfo.Arguments = "\"" + link.NavigateUri + "\"";
            process.Start();
        }

        private static string GetBrowserPath()
        {
            string browser = String.Empty;

            try
            {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false) ??
                                  Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http", false);

                if (key != null)
                {
                    browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                    if (!browser.EndsWith("exe", StringComparison.Ordinal))
                    {
                        browser = browser.Substring(0, browser.LastIndexOf(".exe", StringComparison.Ordinal) + 4);
                    }

                    key.Close();
                }
            }
            catch
            {
                return String.Empty;
            }

            return browser;
        }
    }
}
