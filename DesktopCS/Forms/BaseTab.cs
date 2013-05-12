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

            this.Controls.Add(TextBox);
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;

            int selectionStart = this.TextBox.SelectionStart;
            int selectionLength = this.TextBox.SelectionLength;

            int charPosition = this.TextBox.GetCharIndexFromPosition(e.Location);
            int lineNumber = this.TextBox.GetLineFromCharIndex(charPosition);
            int lineStart = this.TextBox.GetFirstCharIndexFromLine(lineNumber);
            int lineEnd = this.TextBox.GetFirstCharIndexFromLine(lineNumber + 1) - 1;

            if (lineEnd < 0)
            {
                lineEnd = this.TextBox.Text.Length;
            }

            this.TextBox.Select(lineStart, lineEnd - lineStart);

            string line = this.TextBox.SelectedText;

            if (line.Length == 0)
            {
                return;
            }

            int wordEnd = line.IndexOf(' ', charPosition - lineStart);

            if (wordEnd < 0)
            {
                wordEnd = line.Length;
            }

            int wordStart = line.Substring(0, wordEnd).LastIndexOf(' ');

            string word = line.Substring(wordStart + 1, wordEnd - wordStart - 1);
            string timeStamp = line.Substring(0, line.IndexOf(' '));

            if (word == timeStamp)
            {
                return;
            }

            string rtfLine = this.TextBox.SelectedRtf;
            string rtfNoEnd = rtfLine.Substring(0, rtfLine.Length - 3);

            this.TextBox.Select(selectionStart, selectionLength);

            int timeStampLocation = rtfLine.IndexOf(timeStamp);
            string rtfOnlyLine = rtfNoEnd.Substring(timeStampLocation + timeStamp.Length);

            int wordLocation = rtfOnlyLine.IndexOf(word);

            if (wordLocation != rtfOnlyLine.LastIndexOf(word))
            {
                string tempLine = rtfOnlyLine.Substring(wordStart);
                if (tempLine.IndexOf(word) == tempLine.LastIndexOf(word))
                {
                    wordLocation = wordStart + tempLine.IndexOf(word);
                }
            }

            if (rtfOnlyLine.Substring(wordLocation - 4, 3) == "\\v0")
            {
                string rtfData = rtfOnlyLine.Substring(0, wordLocation - 5);
                string rtfHidden = rtfData.Substring(rtfData.LastIndexOf(' ') + 1);

                string hiddenCommand = rtfHidden.Substring(0, rtfHidden.IndexOf(':'));

                this.Cursor = Cursors.Cross;

                if (hiddenCommand == "cs-pm")
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    MessageBox.Show(hiddenCommand);
                }
            }
            else
            {
                return;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.FillRectangle(new SolidBrush(Constants.CHAT_BACKGROUND_COLOR), this.DisplayRectangle);
        }
    }
}
