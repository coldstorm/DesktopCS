using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DesktopCS.Forms;
using NetIRC;

namespace DesktopCS
{
    class ChatOutput
    {
        public BaseTab Tab;

        public ChatOutput(BaseTab tab)
        {
            this.Tab = tab;
        }

        public void AddLine(User author, string text)
        {
            RichTextBox textBox = this.Tab.Controls["TextBox"] as RichTextBox;

            if (!textBox.Rtf.EndsWith("\\par\n}"))
            {
                textBox.Rtf.Insert(textBox.Rtf.LastIndexOf('}'), "\\par");
            }

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();

            List<Color> colorTable = ChatOutput.GetColorTable(textBox.Rtf).ToList();

            if (colorTable == null || !colorTable.Contains(Constants.TIMESTAMP_COLOR))
            {
                colorTable = new List<Color>();
                colorTable.Add(Constants.TIMESTAMP_COLOR);
            }

            int timestampIndex = colorTable.IndexOf(Constants.TIMESTAMP_COLOR) + 1;

            Color userColor = UserNode.ColorFromUser(author);
            int colorIndex;

            int textIndex = colorTable.IndexOf(Constants.TEXT_COLOR) + 1;

            if (textIndex < 1)
            {
                colorTable.Add(Constants.TEXT_COLOR);
                textIndex = colorTable.Count;
            }

            if (!colorTable.Contains(userColor))
            {
                colorTable.Add(userColor);
                colorIndex = colorTable.Count;
            }
            else
            {
                colorIndex = colorTable.IndexOf(userColor) + 1;
            }

            string message = String.Format("\\cf{0}[{1:HH:mm}] \\cf{2}{3} \\cf{4} {5}",
                timestampIndex, DateTime.Now, colorIndex, author.NickName, textIndex, text);

            string rtf = textBox.Rtf;

            rtf = rtf.Insert(rtf.LastIndexOf('}'), message);
            rtf = SetColorTable(rtf, colorTable.ToArray());

            textBox.Rtf = rtf;

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
        }

        public static Color[] GetColorTable(string rtf)
        {
            int colorTableStart = rtf.IndexOf("\\colortbl");

            if (colorTableStart < 0)
            {
                return new Color[0];
            }

            int colorTableEnd = rtf.IndexOf("}", colorTableStart);

            if (colorTableEnd < 0)
            {
                return new Color[0];
            }

            string colorTable = rtf.Substring(colorTableStart + 10, colorTableEnd - colorTableStart);

            string[] colorTableData = colorTable.Split(';').Skip(1).Reverse().Skip(1).Reverse().ToArray();

            List<Color> colors = new List<Color>();

            foreach (string colorData in colorTableData)
            {
                string[] parts = colorData.Split('\\').Skip(1).ToArray();

                string red = parts[0].Substring(3);
                string green = parts[1].Substring(5);
                string blue = parts[2].Substring(4);

                Color color = Color.FromArgb(255, int.Parse(red), int.Parse(green), int.Parse(blue));

                colors.Add(color);
            }

            return colors.ToArray();
        }

        public static string SetColorTable(string rtf, Color[] colorTable)
        {
            int colorTableStart = rtf.IndexOf("\\colortbl");
            int colorTableEnd = 0;

            if (colorTableStart < 0)
            {
                colorTableStart = rtf.IndexOf("\\rtf") + 5;
                colorTableEnd = colorTableStart;
            }
            else
            {
                colorTableEnd = rtf.IndexOf('}', colorTableStart);
            }

            string newCt = "\\colortbl;";

            foreach (Color color in colorTable)
            {
                newCt += String.Format("\\red{0}\\green{1}\\blue{2};", color.R, color.G, color.B);
            }

            rtf = rtf.Remove(colorTableStart, colorTableEnd - colorTableStart);
            rtf = rtf.Insert(colorTableStart, newCt);

            return rtf;
        }
    }
}
