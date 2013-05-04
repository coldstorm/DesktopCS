using System;
using System.Windows.Forms;

namespace DesktopCS.Forms
{
    class CSTabControl : TabControl
    {
        public CSTabControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
        }
    }
}
