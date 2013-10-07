using System;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;

namespace DesktopCS.Helpers.Parsers
{
    public static class MIRCHelper
    {
        public const char BoldChar = (char)2;
        public const char ItalicChar = (char)29;
        public const char UnderlineChar = (char)31;
        public const char ReverseChar = (char)18;
        public const char InverseChar = (char)22;
        public const char ResetChar = (char)15;
        public const char ColorChar = (char)3;
        
        public static Span Parse(string text, ParseArgs args, Func<string, Inline> callback)
        {
            string buffer = String.Empty;
            var span = new Span();

            bool isBold = false;
            bool isItalic = false;
            bool isUnderline = false;

            int foreground = -1;
            int background = -1;

            var flushBuffer = new Action(() =>
            {
                // ReSharper disable AccessToModifiedClosure
                span.Inlines.Add(GetInline(buffer, isBold, isItalic, isUnderline, foreground, background, args, callback));
                buffer = String.Empty;
                // ReSharper restore AccessToModifiedClosure
            });

            var resetColors = new Action(() =>
            {
                // ReSharper disable AccessToModifiedClosure
                foreground = -1;
                background = -1;
                // ReSharper restore AccessToModifiedClosure
            });

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                switch (c)
                {
                    case BoldChar:
                        flushBuffer();
                        isBold = !isBold;
                        break;

                    case ItalicChar:
                        flushBuffer();
                        isItalic = !isItalic;
                        break;

                    case UnderlineChar:
                        flushBuffer();
                        isUnderline = !isUnderline;
                        break;

                    case ReverseChar: case InverseChar:
                        flushBuffer();
                        int tempInt = foreground;
                        foreground = background;
                        background = tempInt;
                        break;

                    case ResetChar:
                        flushBuffer();

                        isBold = false;
                        isItalic = false;
                        isUnderline = false;

                        resetColors();
                        break;

                    case ColorChar:
                        flushBuffer();
                        resetColors();

                        Match matches = Regex.Match(text.Substring(i + 1),
                            @"^(?<foreground>1[0-5]|0[0-9]|[0-9])?(,(?<background>1[0-5]|0[0-9]|[0-9]))?",
                            RegexOptions.ExplicitCapture);

                        if (matches.Success)
                        {
                            string foregroundStr = matches.Groups["foreground"].Value;
                            if (!String.IsNullOrEmpty(foregroundStr))
                            {
                                foreground = int.Parse(foregroundStr);
                                i += foregroundStr.Length; // skip number
                            }

                            string backgroundStr = matches.Groups["background"].Value;
                            if (!String.IsNullOrEmpty(backgroundStr))
                            {
                                background = int.Parse(backgroundStr);
                                i++; // skip comma
                                i += backgroundStr.Length; // skip number
                            }
                        }
                        break;

                    default:
                        buffer += c;
                        continue;
                }
            }
            // flush remaining characters
            flushBuffer();

            return span;
        }

        private static Color GetColor(int color, Color defaultColor)
        {
            // Source: http://www.ircbeginner.com/ircinfo/colors.html
            switch (color)
            {
                case 0:
                    return Colors.White;
                case 1:
                    return Colors.Black;
                case 2:
                    return Colors.Navy;
                case 3:
                    return Colors.Green;
                case 4:
                    return Colors.Red;
                case 5:
                    return Colors.Brown;
                case 6:
                    return Colors.Purple;
                case 7:
                    return Colors.Olive;
                case 8:
                    return Colors.Yellow;
                case 9:
                    return Colors.LimeGreen;
                case 10:
                    return Colors.Teal;
                case 11:
                    return Colors.Aqua;
                case 12:
                    return Colors.RoyalBlue;
                case 13:
                    return Colors.HotPink;
                case 14:
                    return Colors.DarkGray;
                case 15:
                    return Colors.LightGray;
                default:
                    return defaultColor;
            }
        }

        private static Inline GetInline(string text, bool isBold, bool isItalic, bool underline, int foreground, int background, ParseArgs args, Func<string, Inline> callback)
        {
            // Search and add hyperlinks
            Inline inline = callback(text);
            inline.Foreground = new SolidColorBrush(GetColor(foreground, args.Forecolor));
            inline.Background = new SolidColorBrush(GetColor(background, Colors.Transparent));

            if (foreground != -1 && foreground == background)
            {
                inline.MouseEnter += inline_InvertColor;
                inline.MouseLeave += inline_InvertColor;
                inline.MouseDown += inline_InvertColor;
                inline.MouseUp += inline_InvertColor;
            }

            if (isBold)
                inline = new Bold(inline);
            if (isItalic)
                inline = new Italic(inline);
            if (underline)
                inline = new Underline(inline);

            return inline;
        }

        private static void inline_InvertColor(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var inline = (Inline)sender;
            var brush = (SolidColorBrush)inline.Background;

            if (inline.IsMouseOver || e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                brush = InvertBrush(brush);

            inline.Foreground = brush;
        }

        private static Color InvertColor(Color color)
        {
            return Color.FromRgb(
                (byte)(255 - color.R), 
                (byte)(255 - color.G), 
                (byte)(255 - color.B));
        }

        private static SolidColorBrush InvertBrush(SolidColorBrush brush)
        {
            return new SolidColorBrush(InvertColor(brush.Color));
        }
    }
}
