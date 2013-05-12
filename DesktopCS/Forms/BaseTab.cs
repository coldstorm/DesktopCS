using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopCS.Forms
{
    [System.ComponentModel.DesignerCategory("")]
    class BaseTab : TabPage
    {
        public TabType Type;

        public RichTextBox TextBox;

        public bool Active = false;

        public BaseTab(string title)
        {
            this.Name = title;
            this.Text = title;

            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            TextBox = new RichTextBox();
            TextBox.Name = "TextBox";
            TextBox.Dock = DockStyle.Fill;
            TextBox.BorderStyle = BorderStyle.None;
            TextBox.BackColor = Constants.CHAT_BACKGROUND_COLOR;
            TextBox.ForeColor = Constants.TEXT_COLOR;
            TextBox.Font = new System.Drawing.Font("Verdana", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, 0);
            TextBox.ReadOnly = true;

            TextBox.MouseMove += TextBox_MouseMove;
            TextBox.DoubleClick += TextBox_DoubleClick;

            this.Controls.Add(TextBox);
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string line = GetLineAtLocation(this.TextBox, e.Location);

            if (line == null)
            {
                return;
            }

            string word = GetWordAtLocation(this.TextBox, e.Location);
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

        void TextBox_DoubleClick(object sender, EventArgs e)
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

            string hiddenCommand = CommandForWord(rtfOnlyLine, word);

            if (hiddenCommand == "cs-pm")
            {
                MessageBox.Show("Send pm to: " + word);
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

                string hiddenCommand = rtfHidden.Substring(0, rtfHidden.IndexOf(':'));

                return hiddenCommand;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.FillRectangle(new SolidBrush(Constants.CHAT_BACKGROUND_COLOR), this.DisplayRectangle);
        }
    }
}
