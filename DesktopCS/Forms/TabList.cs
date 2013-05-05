using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopCS.Forms
{
    class TabList : TabControl
    {
        public TabList()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            this.DrawBackground(g);
            this.DrawTabs(g);
        }

        private void DrawTabs(Graphics g)
        {
            for (int i = 0; i < this.TabCount; i++)
            {
                this.DrawTab(i, g);
            }
        }

        private void DrawTab(int index, Graphics g)
        {
            Color bgColor = Constants.INACTIVE_TAB_BACKGROUND_COLOR;
            Color textColor = Constants.INACTIVE_TAB_TEXT_COLOR;
            Color borderColor = Constants.INACTIVE_TAB_BORDER_COLOR;

            BaseTab tabPage = this.TabPages[index] as BaseTab;

            Rectangle borderRect = this.GetTabRect(index);
            borderRect.X += index * 3 + 2;

            Rectangle innerRect = borderRect;
            innerRect.Height += 1;

            if (this.SelectedIndex == index)
            {
                innerRect.Height += 1;

                bgColor = Constants.TAB_BACKGROUND_COLOR;
                textColor = Constants.TAB_TEXT_COLOR;
                borderColor = Constants.TAB_BORDER_COLOR;
            }

            g.DrawRectangle(new Pen(borderColor), borderRect);
            g.FillRectangle(new SolidBrush(bgColor), innerRect);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            g.DrawString(tabPage.Text, SystemFonts.DefaultFont, new SolidBrush(textColor), borderRect, format);
        }

        private void DrawBackground(Graphics g)
        {
            Rectangle backRect = this.Bounds;
            backRect.Height += this.Bounds.Y;
            backRect.Y = 0;

            Rectangle borderRect = this.Bounds;
            borderRect.Width -= 7;
            borderRect.Height -= 28;
            borderRect.X += 3;

            g.FillRectangle(new SolidBrush(Constants.BACKGROUND_COLOR), backRect);
            g.DrawRectangle(new Pen(Constants.TAB_BORDER_COLOR), borderRect);
        }
    }
}
