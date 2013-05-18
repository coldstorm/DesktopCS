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
            Color authorColor = UserNode.ColorFromUser(author);

            string colorHex = string.Format("{0:X6}", authorColor.ToArgb() & 0xFFFFFF);

            authorElement.InnerText = author.NickName;
            authorElement.Style = "color:#" + colorHex + ";text-decoration:none;";
            authorElement.SetAttribute("href", "cs-pm:" + author.NickName);

            HtmlElement textElement = browser.Document.CreateElement("span");

            textElement.InnerText = " " + text;

            line.AppendChild(timeStamp);
            line.AppendChild(authorElement);
            line.AppendChild(textElement);

            browser.Document.Body.AppendChild(line);

            return;
        }
    }
}
