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
        private Client Client;

        public ChatOutput(BaseTab tab, Client client)
        {
            this.Tab = tab;
            this.Client = client;
        }

        private void AddLine(string text, List<Color> colorTable)
        {
            RichTextBox textBox = this.Tab.Controls["TextBox"] as RichTextBox;

            int timestampIndex = GetColorIndex(colorTable, Constants.TIMESTAMP_COLOR);

            int textIndex = GetColorIndex(colorTable, Constants.TEXT_COLOR);

            string[] words = text.Split(' ');

            for (int i = 1; i < words.Length; i++)
            {
                if (words[i - 1] == "\\v0")
                {
                    continue;
                }

                foreach (Channel channel in this.Client.Channels.Values)
                {
                    if (channel.Users.ContainsKey(words[i]))
                    {
                        words[i] = "{\\v cs-pm:" + words[i] + " }" + words[i];

                        break;
                    }
                }
            }

            text = string.Join(" ", words);

            string message = string.Format("\\cf{0} [{1:HH:mm}] \\cf{2} {3}",
                timestampIndex, DateTime.Now, textIndex, text);

            string rtf = textBox.Rtf;

            if (rtf.EndsWith("\\fs15\\par\r\n}\r\n"))
            {
                rtf = rtf.Replace("\\par\r\n}", "}");
            }

            rtf = rtf.Insert(rtf.LastIndexOf("}"), message);
            rtf = SetColorTable(rtf, colorTable.ToArray());

            textBox.Rtf = rtf;

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
        }

        public void AddLine(string text)
        {
            WebBrowser browser = this.Tab.Browser;

            HtmlElement line = browser.Document.CreateElement("div");
            line.InnerText = string.Format("[{0:HH:mm}] {1}", DateTime.Now, text);

            browser.Document.Body.AppendChild(line);

            return;
        }

        public void AddLine(User author, string text)
        {
            WebBrowser browser = this.Tab.Browser;

            HtmlElement line = browser.Document.CreateElement("div");

            HtmlElement timeStamp = browser.Document.CreateElement("span");

            timeStamp.InnerText = string.Format("[{0:HH:mm}] ", DateTime.Now);

            HtmlElement authorElement = browser.Document.CreateElement("a");

            authorElement.InnerText = author.NickName;
            authorElement.Style = "color:red;text-decoration:none;";
            authorElement.SetAttribute("href", "cs-pm:" + author.NickName);

            HtmlElement textElement = browser.Document.CreateElement("span");

            textElement.InnerText = " " + text;

            line.AppendChild(timeStamp);
            line.AppendChild(authorElement);
            line.AppendChild(textElement);

            browser.Document.Body.AppendChild(line);

            return;

            RichTextBox textBox = this.Tab.Controls["TextBox"] as RichTextBox;

            List<Color> colorTable = ChatOutput.GetColorTable(textBox.Rtf).ToList();

            Color userColor = UserNode.ColorFromUser(author);
            int colorIndex = GetColorIndex(colorTable, userColor);

            int textIndex = GetColorIndex(colorTable, Constants.TEXT_COLOR);

            string message = String.Format("\\cf{0}{{\\v cs-pm:{2} }}{1}{2} \\cf{3} {4}",
                colorIndex, UserNode.RankChars[author.Rank], author.NickName, textIndex, text);

            AddLine(message, colorTable);
        }

        public static Color[] GetColorTable(string rtf)
        {
            int colorTableStart = rtf.IndexOf("\\colortbl");

            if (colorTableStart < 0)
            {
                return new Color[0];
            }

            int colorTableEnd = rtf.IndexOf('}', colorTableStart);

            if (colorTableEnd < 0)
            {
                return new Color[0];
            }

            string colorTable = rtf.Substring(colorTableStart + 11, colorTableEnd - colorTableStart - 12);

            string[] colorTableData = colorTable.Split(';');

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

        public static int GetColorIndex(List<Color> colorTable, Color color)
        {
            if (colorTable.Contains(color))
            {
                return colorTable.IndexOf(color) + 1;
            }

            colorTable.Add(color);

            return colorTable.Count;
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
