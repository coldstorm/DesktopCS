using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Services.IRC.Messages.Send;
using Microsoft.Win32;

namespace DesktopCS.Helpers.Parsers
{
    public static class URLHelper
    {
        public static Span Parse(string text, ParseArgs args, Func<string, Inline> callback)
        {
            return RegexHelper.Parse(text, _urlRegex, args, callback,
                s =>
                {
                    var link = new Hyperlink(new Run(s))
                    {
                        NavigateUri = new Uri(s),
                        Foreground = new SolidColorBrush(ColorHelper.HyperlinkColor),
                        TextDecorations = null
                    };

                    link.Click += OnUrlClick;

                    return link;
                });
        }

        private static readonly Regex _urlRegex = new Regex(@"(https?|ftp)://[^\s/$.?#].[^\s]*");

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
