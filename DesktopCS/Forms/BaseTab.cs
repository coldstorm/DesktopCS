using System;
using System.Drawing;
using System.Windows.Forms;
using NetIRC;

namespace DesktopCS.Forms
{
    [System.ComponentModel.DesignerCategory("")]
    class BaseTab : TabPage
    {
        public TabType Type;

        public RichTextBox TextBox;
        public WebBrowser Browser;

        public bool Active = false;

        public BaseTab(string title)
        {
            this.Name = title;
            this.Text = title;

            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            Browser = new WebBrowser();
            Browser.Dock = DockStyle.Fill;
            Browser.Name = "Browser";

            Browser.DocumentText = "<html><body></body></html>";

            while (Browser.Document.Body == null)
            {
                Application.DoEvents();
            }

            Browser.Document.ForeColor = Constants.TEXT_COLOR;
            Browser.Document.BackColor = Constants.CHAT_BACKGROUND_COLOR;

            Browser.Document.Body.Style = "font-size:10px;font-family:verdana;margin:0;padding:0;";

            Browser.Navigating += (s, e) =>
            {
                e.Cancel = true;

                switch (e.Url.Scheme)
                {
                    case "cs-pm":
                        MainForm mainForm = this.Parent.Parent as MainForm;

                        string userName = e.Url.LocalPath;
                        User user = null;

                        foreach (Channel channel in mainForm.Client.Channels.Values)
                        {
                            if (channel.Users.ContainsKey(userName))
                            {
                                user = channel.Users[userName];
                                break;
                            }
                        }

                        if (user == null)
                        {
                            break;
                        }

                        PrivateMessageTab tab = new PrivateMessageTab(user);

                        mainForm.AddTab(tab);

                        break;

                    case "cs-channel":
                        // TODO: Switch to or open up the channel tab

                        break;

                    default:
                        // TODO: Open the web browser pointing to the url

                        MessageBox.Show(e.Url.Scheme);

                        break;
                }
            };

            this.Controls.Add(Browser);
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string line = GetLineAtLocation(this.TextBox, e.Location);

            if (line == null)
            {
                return;
            }

            string word = GetWordAtLocation(this.TextBox, e.Location);

            if (word.Length == 0)
            {
                return;
            }

            string timeStamp = line.Substring(0, line.IndexOf(' '));

            if (word == timeStamp)
            {
                return;
            }

            string rtfLine = GetRtfAtLocation(this.TextBox, e.Location);
            string rtfNoEnd = rtfLine.Substring(0, rtfLine.Length - 3);

            int timeStampLocation = rtfLine.IndexOf(timeStamp);
            string rtfOnlyLine = rtfNoEnd.Substring(timeStampLocation + timeStamp.Length);

            string hiddenCommand = CommandForWord(rtfOnlyLine, word);

            if (hiddenCommand == "cs-pm")
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }

            this.TextBox.SelectionLength = 0;
        }

        private void TextBox_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs evt = e as MouseEventArgs;

            string line = GetLineAtLocation(this.TextBox, evt.Location);

            if (line == null)
            {
                return;
            }

            string word = GetWordAtLocation(this.TextBox, evt.Location);
            string timeStamp = line.Substring(0, line.IndexOf(' '));

            if (word == timeStamp)
            {
                return;
            }

            string rtfLine = GetRtfAtLocation(this.TextBox, evt.Location);
            string rtfNoEnd = rtfLine.Substring(0, rtfLine.Length - 3);

            int timeStampLocation = rtfLine.IndexOf(timeStamp);
            string rtfOnlyLine = rtfNoEnd.Substring(timeStampLocation + timeStamp.Length);

            string hiddenData = CommandForWord(rtfOnlyLine, word);
            string hiddenCommand = hiddenData.Split(':')[0];

            if (hiddenCommand == "cs-pm")
            {
                string commandTarget = hiddenData.Split(':')[1];

                MessageBox.Show("Send pm to: " + commandTarget);
            }

            this.TextBox.SelectionLength = 0;
        }

        private static string GetLineAtLocation(RichTextBox textBox, Point location)
        {
            int charPosition = textBox.GetCharIndexFromPosition(location);
            int lineNumber = textBox.GetLineFromCharIndex(charPosition);
            int lineStart = textBox.GetFirstCharIndexFromLine(lineNumber);
            int lineEnd = textBox.GetFirstCharIndexFromLine(lineNumber + 1) - 1;

            if (lineEnd < 0)
            {
                lineEnd = textBox.Text.Length;
            }

            textBox.Select(lineStart, lineEnd - lineStart);

            string line = textBox.SelectedText;

            if (line.Length == 0)
            {
                return null;
            }

            return line;
        }

        private static string GetRtfAtLocation(RichTextBox textBox, Point location)
        {
            string line = GetLineAtLocation(textBox, location);

            return textBox.SelectedRtf;
        }

        private static string GetWordAtLocation(RichTextBox textBox, Point location)
        {
            int charPosition = textBox.GetCharIndexFromPosition(location);
            int lineNumber = textBox.GetLineFromCharIndex(charPosition);
            int lineStart = textBox.GetFirstCharIndexFromLine(lineNumber);

            string line = GetLineAtLocation(textBox, location);

            if (line == null)
            {
                return null;
            }

            int wordEnd = line.IndexOf(' ', charPosition - lineStart);

            if (wordEnd < 0)
            {
                wordEnd = line.Length;
            }

            int wordStart = line.Substring(0, wordEnd).LastIndexOf(' ');

            string word = line.Substring(wordStart + 1, wordEnd - wordStart - 1);

            return word;
        }

        private static string CommandForWord(string rtfLine, string word)
        {
            int offset = -word.Length;

            while (true)
            {
                offset += word.Length;

                int position = rtfLine.IndexOf(word, offset);

                if (position < 0)
                {
                    return null;
                }

                if (position < 5)
                {
                    continue;
                }

                string check = rtfLine.Substring(position - 4, 3);

                if (check != "\\v0")
                {
                    continue;
                }

                string rtfData = rtfLine.Substring(0, position - 5);
                string rtfHidden = rtfData.Substring(rtfData.LastIndexOf(' ') + 1);

                return rtfHidden;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.FillRectangle(new SolidBrush(Constants.CHAT_BACKGROUND_COLOR), this.DisplayRectangle);
        }
    }
}
