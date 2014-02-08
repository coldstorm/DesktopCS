using System;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Helpers.Parsers;
using DesktopCS.Models;

namespace DesktopCS.Helpers.Extensions
{
    public static class ChatLineExtensions
    {
        public static Paragraph ToParagraph(this ChatLine line)
        {
            var p = new Paragraph();

            // Time
            if (!String.IsNullOrEmpty(line.Timestamp))
            {
                Color timeColor = ColorHelper.TimeColor;

                var timeRun = new Run(line.Timestamp) { Foreground = new SolidColorBrush(timeColor) };
                p.Inlines.Add(timeRun);
                p.Inlines.Add(" ");
            }

            // User
            if (line.User != null)
            {
                Color color = line.User.Metadata.Color;

                var rankRun = new Run(NetIRCHelper.RankChars[line.User.HighestRank].ToString())
                {
                    Foreground = new SolidColorBrush(color)
                };

                var usernameRun = new Hyperlink(new Run(line.User.NickName))
                {
                    Tag = line.Args.QueryCallback,
                    Foreground = new SolidColorBrush(color),
                    TextDecorations = null
                };
                usernameRun.Click += usernameRun_Click;

                p.Inlines.Add(rankRun);
                p.Inlines.Add(usernameRun);
                p.Inlines.Add(" ");
            }

            // Message
            {
                line.Args.Forecolor = line.ChatColor;

                Span chatSpan = OutputHelper.Parse(line.Chat, line.Args);
                p.Inlines.Add(chatSpan);
            }

            return p;
        }

        private static void usernameRun_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var link = (Hyperlink) sender;
            var run = (Run) link.Inlines.FirstInline;
            var callback = (Action<string>) link.Tag;
            callback(run.Text);
        }
    }
}
