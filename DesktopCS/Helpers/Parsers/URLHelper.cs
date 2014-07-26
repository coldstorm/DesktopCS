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
            try
            {
                // Let Process.Start() open the link in the appropriate browser
                Process.Start(link.NavigateUri.AbsoluteUri);
            }

            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
