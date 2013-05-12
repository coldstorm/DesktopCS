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

            this.Controls.Add(TextBox);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.FillRectangle(new SolidBrush(Constants.CHAT_BACKGROUND_COLOR), this.DisplayRectangle);
        }
    }
}
