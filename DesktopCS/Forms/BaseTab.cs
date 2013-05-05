using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopCS.Forms
{
    class BaseTab : TabPage
    {
        public TabType Type;

        public BaseTab(string title)
        {
            this.Name = title;
            this.Text = title;

            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.FillRectangle(new SolidBrush(Constants.CHAT_BACKGROUND_COLOR), this.DisplayRectangle);
        }
    }
}
