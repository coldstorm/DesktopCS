using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DesktopCS
{
    public partial class CTabPage : TabPage
    {
        public CTabPage(string _name)
        {
            this.Text = _name;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            //e.Graphics.DrawRectangle(new Pen(Constants.TAB_BORDER_COLOR), this.DisplayRectangle);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Rectangle rect = this.DisplayRectangle;
            //rect.X -= 4;
            rect.Y -= 1;
            //rect.Width += 2;
            rect.Height += 2;
            
            e.Graphics.FillRectangle(new SolidBrush(Constants.CHAT_BACKGROUND_COLOR), rect);
        }
    }
}
