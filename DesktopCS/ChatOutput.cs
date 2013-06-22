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

            authorElement.InnerText = string.Format("{0}{1} ", UserNode.RankChars[author.Rank], author.NickName);
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
                if (words[i] == "::" || words[i].StartsWith("::") || words[i].EndsWith("::") && Properties.Settings.Default.Spoilers)
                {
                    if (words[i].StartsWith("::") && words[i].EndsWith("::") && words[i].Length > 2) //Single word in a spoiler
                    {
                        textElement = browser.Document.CreateElement("span");
                        words[i] = words[i].Replace("::", "");
                        textElement.InnerText = words[i];
                        textElement.Style = "background-color:#000000;color:#000000";
                        textElement.MouseOver += spoiler_MouseOver;
                        textElement.MouseLeave += spoiler_MouseLeave;

                        line.AppendChild(textElement);
                    }

                    else
                    {
                        spoiler++;

                        if (spoiler % 2 == 0)
                        {
                            spoilerEnd = i;
                            words[spoilerStart] = words[spoilerStart].Replace("::", "");
                            words[spoilerEnd] = words[spoilerEnd].Replace("::", "");

                            textElement = browser.Document.CreateElement("span");
                            string spoilerText = "";
                            for (int j = spoilerStart; j <= spoilerEnd; j++)
                            {
                                spoilerText += words[j] + " ";
                            }
                            textElement.InnerText = spoilerText.Trim();
                            textElement.Style = "background-color:#000000;color:#000000";
                            textElement.MouseOver += spoiler_MouseOver;
                            textElement.MouseLeave += spoiler_MouseLeave;

                            line.AppendChild(textElement);

                            textElement = browser.Document.CreateElement("span");
                            textElement.InnerText = " ";
                            line.AppendChild(textElement);

                            spoilerStart = -1;
                            spoilerEnd = -1;
                        }

                        else
                        {
                            spoilerStart = i;
                        }
                    }
                }

                else if (spoilerStart == -1 && spoilerEnd == -1)
                {
                    if (ParseChannelName(words[i]) != null)
                    {
                        line.AppendChild(ParseChannelName(words[i]));
                    }

                    else if (ParseNickName(words[i]) != null)
                    {
                        line.AppendChild(ParseNickName(words[i]));
                    }

                    else if (ParseLink(words[i]) != null)
                    {
                        line.AppendChild(ParseLink(words[i]));
                    }

                    else
                    {
                        textElement = browser.Document.CreateElement("span");
                        textElement.InnerText += words[i] + " ";
                        line.AppendChild(textElement);
                    }
                }
            }

            browser.Document.Body.AppendChild(line);

            browser.Document.Window.ScrollTo(0, browser.Document.Body.ScrollRectangle.Bottom);
        }

        public void AddActionLine(User author, string action)
        {
            WebBrowser browser = this.Tab.Browser;

            HtmlElement line = browser.Document.CreateElement("div");
            line.Style = "font-style:italic";

            string timeStampColorHex = string.Format("{0:X6}", Constants.TIMESTAMP_COLOR.ToArgb() & 0xFFFFFF);

            HtmlElement timeStamp = browser.Document.CreateElement("span");

            timeStamp.InnerText = string.Format("[{0:HH:mm}] ", DateTime.Now);
            timeStamp.Style = "color:#" + timeStampColorHex + ";font-style:normal";

            HtmlElement authorElement = browser.Document.CreateElement("a");
            Color authorColor = UserNode.ColorFromUser(author);

            string authorColorHex = string.Format("{0:X6}", authorColor.ToArgb() & 0xFFFFFF);

            authorElement.InnerText = "* " + author.NickName + " ";
            authorElement.Style = "color:#" + authorColorHex + ";text-decoration:none;font-style:inherit";
            authorElement.SetAttribute("href", "cs-pm:" + author.NickName);

            line.AppendChild(timeStamp);
            line.AppendChild(authorElement);

            string[] words = action.Split(' ');

            HtmlElement textElement = browser.Document.CreateElement("span");
            textElement.InnerText = " ";
            textElement.Style = "font-style:inherit";

            int spoiler = 0;
            int spoilerStart = -1;
            int spoilerEnd = -1;
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "::" || words[i].StartsWith("::") || words[i].EndsWith("::") && Properties.Settings.Default.Spoilers)
                {
                    if (words[i].StartsWith("::") && words[i].EndsWith("::") && words[i].Length > 2) //Single word in a spoiler
                    {
                        textElement = browser.Document.CreateElement("span");
                        words[i] = words[i].Replace("::", "");
                        textElement.InnerText = words[i];
                        textElement.Style = "background-color:#000000;color:#000000";
                        textElement.MouseOver += spoiler_MouseOver;
                        textElement.MouseLeave += spoiler_MouseLeave;

                        line.AppendChild(textElement);
                    }

                    else
                    {
                        spoiler++;

                        if (spoiler % 2 == 0)
                        {
                            spoilerEnd = i;
                            words[spoilerStart] = words[spoilerStart].Replace("::", "");
                            words[spoilerEnd] = words[spoilerEnd].Replace("::", "");

                            textElement = browser.Document.CreateElement("span");
                            string spoilerText = "";
                            for (int j = spoilerStart; j <= spoilerEnd; j++)
                            {
                                spoilerText += words[j] + " ";
                            }
                            textElement.InnerText = spoilerText.Trim();
                            textElement.Style = "background-color:#000000;color:#000000";
                            textElement.MouseOver += spoiler_MouseOver;
                            textElement.MouseLeave += spoiler_MouseLeave;

                            line.AppendChild(textElement);

                            textElement = browser.Document.CreateElement("span");
                            textElement.InnerText = " ";
                            line.AppendChild(textElement);

                            spoilerStart = -1;
                            spoilerEnd = -1;
                        }

                        else
                        {
                            spoilerStart = i;
                        }
                    }
                }

                else if (spoilerStart == -1 && spoilerEnd == -1)
                {
                    if (ParseChannelName(words[i]) != null)
                    {
                        line.AppendChild(ParseChannelName(words[i]));
                    }

                    else if (ParseNickName(words[i]) != null)
                    {
                        line.AppendChild(ParseNickName(words[i]));
                    }

                    else if (ParseLink(words[i]) != null)
                    {
                        line.AppendChild(ParseLink(words[i]));
                    }

                    else
                    {
                        textElement = browser.Document.CreateElement("span");
                        textElement.InnerText += words[i] + " ";
                        line.AppendChild(textElement);
                    }
                }
            }

            browser.Document.Body.AppendChild(line);

            browser.Document.Window.ScrollTo(0, browser.Document.Body.ScrollRectangle.Bottom);
        }

        private HtmlElement ParseNickName(string word)
        {
            WebBrowser browser = this.Tab.Browser;
            HtmlElement userElement = browser.Document.CreateElement("a");

            foreach (Channel channel in this.Client.Channels.Values)
            {
                if (channel.Users.ContainsKey(word))
                {
                    userElement.SetAttribute("href", "cs-pm:" + word);
                    userElement.InnerText = word + " ";

                    if (word == this.Client.User.NickName)
                    {
                        userElement.Style = "text-decoration:none;color:#ff921e;font-weight:bold;font-style:normal;";
                    }

                    else
                    {
                        userElement.Style = "text-decoration:none;color:#babbbf;font-style:inherit;";
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(userElement.InnerText))
                return userElement;

            return null;
        }

        private HtmlElement ParseChannelName(string word)
        {
            WebBrowser browser = this.Tab.Browser;
            HtmlElement channelElement = browser.Document.CreateElement("a");

            if (word.StartsWith("#"))
            {
                word = word.Substring(1);

                channelElement.SetAttribute("href", "cs-channel:" + word);
                channelElement.InnerText = "#" + word + " ";
                channelElement.Style = "text-decoration:none;color:#babbbf;font-style:inherit";
            }

            if (!String.IsNullOrWhiteSpace(channelElement.InnerText))
                return channelElement;

            return null;
        }

        private HtmlElement ParseLink(string word)
        {
            WebBrowser browser = this.Tab.Browser;
            HtmlElement linkElement = browser.Document.CreateElement("a");

            Uri uriResult;

            bool isUri = Uri.TryCreate(word, UriKind.Absolute, out uriResult);

            if (isUri && !string.IsNullOrEmpty(uriResult.LocalPath))
            {
                linkElement.InnerText = word + " ";
                linkElement.SetAttribute("href", uriResult.AbsoluteUri);
                linkElement.Style = "color:#4a7691;font-style:inherit";
            }

            if (!String.IsNullOrWhiteSpace(linkElement.InnerText))
                return linkElement;

            return null;
        }

        void spoiler_MouseLeave(object sender, HtmlElementEventArgs e)
        {
            e.FromElement.Style = "background-color:#000000;color:#000000";
        }

        void spoiler_MouseOver(object sender, HtmlElementEventArgs e)
        {
            e.ToElement.Style = "color:inherit;font-style:inherit;font-weight:inherit";
        }
    }
}
