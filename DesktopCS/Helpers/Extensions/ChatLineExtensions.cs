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

                var usernameRun = new Run(line.User.NickName) { Foreground = new SolidColorBrush(color) };
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
    }
}
