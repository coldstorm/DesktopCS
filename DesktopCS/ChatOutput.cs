using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DesktopCS.Forms;
using NetIRC;

namespace DesktopCS
{
    public class ChatOutput
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

            int spoiler = 0;
            int spoilerStart = -1;
            int spoilerEnd = -1;
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "::" || words[i].StartsWith("::") || words[i].EndsWith("::"))
                {
                    spoiler++;

                    if (spoiler % 2 == 0)
                    {
                        spoilerEnd = i;
                        words[spoilerStart] = words[spoilerStart].Replace("::", "");
                        words[spoilerEnd] = words[spoilerEnd].Replace("::", "");

                        textElement = browser.Document.CreateElement("span");
                        string spoilerText = " ";
                        for (int j = spoilerStart; j <= spoilerEnd; j++)
                        {
                            spoilerText += words[j] + " ";
                        }
                        textElement.InnerText = spoilerText;
                        textElement.Style = "background-color:#000000;color:#000000";
                        textElement.MouseOver += spoiler_MouseOver;
                        textElement.MouseLeave += spoiler_MouseLeave;

                        line.AppendChild(textElement);
                    }

                    else
                    {
                        spoilerStart = i;
                    }
                }

                else
                {
                    textElement = browser.Document.CreateElement("span");
                    if (words[i].StartsWith("#"))
                    {
                        words[i] = words[i].Substring(1);

                        if (!string.IsNullOrWhiteSpace(textElement.InnerText))
                        {
                            textElement.InnerText += " ";
                        }

                        line.AppendChild(textElement);

                        textElement = browser.Document.CreateElement("span");
                        textElement.InnerText = " ";

                        HtmlElement channelElement = browser.Document.CreateElement("a");
                        channelElement.SetAttribute("href", "cs-channel:" + words[i]);
                        channelElement.InnerText = "#" + words[i];
                        channelElement.Style = "text-decoration:none;color:#babbbf;";

                        line.AppendChild(channelElement);

                        continue;
                    }

                    foreach (Channel channel in this.Client.Channels.Values)
                    {
                        if (channel.Users.ContainsKey(words[i]))
                        {
                            if (!string.IsNullOrWhiteSpace(textElement.InnerText))
                            {
                                textElement.InnerText += " ";
                            }

                            line.AppendChild(textElement);

                            textElement = browser.Document.CreateElement("span");
                            textElement.InnerText = " ";

                            HtmlElement userElement = browser.Document.CreateElement("a");
                            userElement.SetAttribute("href", "cs-pm:" + words[i]);
                            userElement.InnerText = words[i];

                            if (words[i] == this.Client.User.NickName)
                            {
                                userElement.Style = "text-decoration:none;color:#ff921e;font-weight:bold";
                            }

                            else
                            {
                                userElement.Style = "text-decoration:none;color:#babbbf;";
                            }

                            line.AppendChild(userElement);

                            goto VbLetsYouBreakThese;
                        }
                    }

                    Uri uriResult;

                    bool isUri = Uri.TryCreate(words[i], UriKind.Absolute, out uriResult);

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
                        linkElement.InnerText = words[i];
                        linkElement.SetAttribute("href", uriResult.AbsoluteUri);
                        linkElement.Style = "color:#4a7691;";

                        line.AppendChild(linkElement);

                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(textElement.InnerText))
                    {
                        textElement.InnerText += " ";
                    }

                    textElement.InnerText += words[i];
                }

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

        void spoiler_MouseLeave(object sender, HtmlElementEventArgs e)
        {
            e.FromElement.Style = "background-color:#000000;color:#000000";
        }

        void spoiler_MouseOver(object sender, HtmlElementEventArgs e)
        {
            e.ToElement.Style = "color:inherit";
        }

        public void AddActionLine(User author, string action)
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

            authorElement.InnerText = "* " + author.NickName;
            authorElement.Style = "color:#" + authorColorHex + ";text-decoration:none;font-style:italic";
            authorElement.SetAttribute("href", "cs-pm:" + author.NickName);

            line.AppendChild(timeStamp);
            line.AppendChild(authorElement);

            string[] words = action.Split(' ');

            HtmlElement textElement = browser.Document.CreateElement("span");
            textElement.InnerText = " ";
            textElement.Style = "font-style:italic";

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
                    channelElement.Style = "text-decoration:none;color:#babbbf;font-style:italic";

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

                        if (word == this.Client.User.NickName)
                        {
                            userElement.Style = "text-decoration:none;color:#ff921e;font-weight:bold";
                        }

                        else
                        {
                            userElement.Style = "text-decoration:none;color:#babbbf;font-style:italic";
                        }

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
                    linkElement.Style = "color:#4a7691;font-style:italic";

                    line.AppendChild(linkElement);

                    continue;
                }

                if (!string.IsNullOrWhiteSpace(textElement.InnerText))
                {
                    textElement.InnerText += " ";
                }

                textElement.InnerText += word;
                textElement.Style = "font-style:italic";

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
