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

            while (tab.Browser.Document.Body == null)
            {
                Application.DoEvents();
            }
        }

        public void AddUserJoin(User user, Channel channel)
        {
            WebBrowser browser = this.Tab.Browser;

            HtmlElement line = browser.Document.CreateElement("div");

            string timeStampColorHex = string.Format("{0:X6}", Constants.TIMESTAMP_COLOR.ToArgb() & 0xFFFFFF);

            HtmlElement timeStamp = browser.Document.CreateElement("span");
            timeStamp.InnerText = string.Format("[{0:HH:mm}] ", DateTime.Now);
            timeStamp.Style = "color:#" + timeStampColorHex;

            line.AppendChild(timeStamp);

            HtmlElement userElement = browser.Document.CreateElement("a");
            string lineColorHex = string.Format("{0:X6}", Constants.TEXT_CONTROL_COLOR.ToArgb() & 0xFFFFFF);

            userElement.InnerText = user.NickName;
            userElement.Style = "color:#" + lineColorHex + ";text-decoration:none;";
            userElement.SetAttribute("href", "cs-pm:" + user.NickName);

            line.AppendChild(userElement);

            HtmlElement messageElement = browser.Document.CreateElement("span");
            messageElement.InnerText = " has joined #" + channel.Name;
            messageElement.Style = "color:#" + lineColorHex;

            line.AppendChild(messageElement);

            browser.Document.Body.AppendChild(line);

            browser.Document.Window.ScrollTo(0, browser.Document.Body.ScrollRectangle.Bottom);
        }

        public void AddLine(string text)
        {
            WebBrowser browser = this.Tab.Browser;

            HtmlElement line = browser.Document.CreateElement("div");

            string timeStampColorHex = string.Format("{0:X6}", Constants.TIMESTAMP_COLOR.ToArgb() & 0xFFFFFF);

            HtmlElement timeStamp = browser.Document.CreateElement("span");
            timeStamp.InnerText = string.Format("[{0:HH:mm}] ", DateTime.Now);
            timeStamp.Style = "color:#" + timeStampColorHex;

            line.AppendChild(timeStamp);

            string lineColorHex = string.Format("{0:X6}", Constants.TEXT_CONTROL_COLOR.ToArgb() & 0xFFFFFF);

            HtmlElement lineText = browser.Document.CreateElement("span");
            lineText.InnerText = text;
            lineText.Style = "color:#" + lineColorHex;

            line.AppendChild(lineText);

            browser.Document.Body.AppendChild(line);

            browser.Document.Window.ScrollTo(0, browser.Document.Body.ScrollRectangle.Bottom);
        }

        public void AddLine(User author, string text)
        {
            WebBrowser browser = this.Tab.Browser;

            HtmlElement line = browser.Document.CreateElement("div");

            string timeStampColorHex = string.Format("{0:X6}", Constants.TIMESTAMP_COLOR.ToArgb() & 0xFFFFFF);

            HtmlElement timeStamp = browser.Document.CreateElement("span");

            timeStamp.InnerText = string.Format("[{0:HH:mm}] ", DateTime.Now);
            timeStamp.Style = "color:#" + timeStampColorHex;

            HtmlElement authorElement = browser.Document.CreateElement("a");
            Color authorColor = UserNode.ColorFromUser(author);

            string authorColorHex = string.Format("{0:X6}", authorColor.ToArgb() & 0xFFFFFF);

            authorElement.InnerText = string.Format("{0}{1}", UserNode.RankChars[author.Rank], author.NickName);
            authorElement.Style = "color:#" + authorColorHex + ";text-decoration:none;";
            authorElement.SetAttribute("href", "cs-pm:" + author.NickName);

            line.AppendChild(timeStamp);
            line.AppendChild(authorElement);

            string[] words = text.Split(' ');

            HtmlElement textElement = browser.Document.CreateElement("span");
            textElement.InnerText = " ";

            foreach (string iterWord in words)
            {
                string word = iterWord;

                if (word.StartsWith("#"))
                {
                    word = word.Substring(1);

                    if (!string.IsNullOrWhiteSpace(textElement.InnerText))
                    {
                        textElement.InnerText += " ";
                    }

                    line.AppendChild(textElement);

                    textElement = browser.Document.CreateElement("span");
                    textElement.InnerText = " ";

                    HtmlElement channelElement = browser.Document.CreateElement("a");
                    channelElement.SetAttribute("href", "cs-channel:" + word);
                    channelElement.InnerText = "#" + word;
                    channelElement.Style = "text-decoration:none;color:#babbbf;";

                    line.AppendChild(channelElement);

                    continue;
                }

                foreach (Channel channel in this.Client.Channels.Values)
                {
                    if (channel.Users.ContainsKey(word))
                    {
                        if (!string.IsNullOrWhiteSpace(textElement.InnerText))
                        {
                            textElement.InnerText += " ";
                        }

                        line.AppendChild(textElement);

                        textElement = browser.Document.CreateElement("span");
                        textElement.InnerText = " ";

                        HtmlElement userElement = browser.Document.CreateElement("a");
                        userElement.SetAttribute("href", "cs-pm:" + word);
                        userElement.InnerText = word;
                        userElement.Style = "text-decoration:none;color:#babbbf;";

                        line.AppendChild(userElement);

                        goto VbLetsYouBreakThese;
                    }
                }

                Uri uriResult;

                bool isUri = Uri.TryCreate(word, UriKind.Absolute, out uriResult);

                if (isUri && !string.IsNullOrEmpty(uriResult.LocalPath))
                {
                    if (!string.IsNullOrWhiteSpace(textElement.InnerText))
                    {
                        textElement.InnerText += " ";
                    }

                    line.AppendChild(textElement);

                    textElement = browser.Document.CreateElement("span");
                    textElement.InnerText = " ";

                    HtmlElement linkElement = browser.Document.CreateElement("a");
                    linkElement.InnerText = word;
                    linkElement.SetAttribute("href", uriResult.AbsoluteUri);
                    linkElement.Style = "color:#4a7691;";

                    line.AppendChild(linkElement);

                    continue;
                }

                if (!string.IsNullOrWhiteSpace(textElement.InnerText))
                {
                    textElement.InnerText += " ";
                }

                textElement.InnerText += word;

            VbLetsYouBreakThese:

                continue;
            }

            if (!string.IsNullOrWhiteSpace(textElement.InnerText))
            {
                line.AppendChild(textElement);
            }

            browser.Document.Body.AppendChild(line);

            browser.Document.Window.ScrollTo(0, browser.Document.Body.ScrollRectangle.Bottom);
        }
    }
}
