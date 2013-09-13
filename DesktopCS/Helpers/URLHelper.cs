using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;

namespace DesktopCS.Helpers
{
    public class URLHelper
    {
        // Copied from http://geekswithblogs.net/casualjim/archive/2005/12/01/61722.aspx
        private static readonly Regex _urlRegex =
            new Regex(
                @"(?#Protocol)(?<domainURL>(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2})))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?");

        public static Inline Parse(string text)
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
                    NavigateUri = new Uri(match.Value, UriKind.RelativeOrAbsolute)
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
