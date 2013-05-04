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
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.DrawRectangle(new Pen(Brushes.DarkBlue), this.DisplayRectangle);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            e.Graphics.FillRectangle(Brushes.LightBlue, this.DisplayRectangle);
        }
    }
}
