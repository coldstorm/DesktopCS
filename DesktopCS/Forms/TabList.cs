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
            BaseTab tabPage = this.TabPages[index] as BaseTab;

            Rectangle borderRect = this.GetTabRect(index);
            borderRect.X += index * 3 + 2;

            Rectangle innerRect = borderRect;
            innerRect.Height += 1;

            if (this.SelectedIndex == index)
            {
                innerRect.Height += 1;
            }

            g.DrawRectangle(new Pen(Constants.TAB_BORDER_COLOR), borderRect);
            g.FillRectangle(new SolidBrush(Constants.CHAT_BACKGROUND_COLOR), innerRect);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            g.DrawString(tabPage.Text, SystemFonts.DefaultFont, new SolidBrush(Constants.TAB_TEXT_COLOR), borderRect, format);
        }

        private void DrawBackground(Graphics g)
        {
            Rectangle backRect = this.Bounds;
            backRect.Height += this.Bounds.Y;
            backRect.Y = 0;

            g.FillRectangle(new SolidBrush(Constants.BACKGROUND_COLOR), backRect);
            g.DrawRectangle(new Pen(Constants.TAB_BORDER_COLOR), this.Bounds);
        }
    }
}
