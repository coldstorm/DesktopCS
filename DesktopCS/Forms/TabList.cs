using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DesktopCS.Forms
{
    [System.ComponentModel.DesignerCategory("")]
    class TabList : TabControl
    {
        public Dictionary<string, BaseTab> Tabs = new Dictionary<string,BaseTab>();

        public TabList()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        public void AddTab(BaseTab tab)
        {
            this.Tabs.Add(tab.Name, tab);

            if (this.Tabs.Count == 1)
            {
                this.TabPages.Add(tab);
                this.SwitchToTab(0);
            }

            this.Invalidate();
        }

        public void RemoveTab(BaseTab tab)
        {
            this.Tabs.Remove(tab.Name);

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            this.DrawBackground(g);
            this.DrawTabs(g);
        }

        private void DrawTabs(Graphics g)
        {
            for (int i = 0; i < this.Tabs.Count; i++)
            {
                this.DrawTab(i, g);
            }
        }

        private void DrawTab(int index, Graphics g)
        {
            Color bgColor = Constants.INACTIVE_TAB_BACKGROUND_COLOR;
            Color textColor = Constants.INACTIVE_TAB_TEXT_COLOR;
            Color borderColor = Constants.INACTIVE_TAB_BORDER_COLOR;

            BaseTab tabPage = this.Tabs.ElementAt(index).Value as BaseTab;

            Rectangle borderRect = this.GetTabRectangle(index);

            Rectangle innerRect = borderRect;
            innerRect.Height += 1;

            if (this.TabCount > 0 && this.TabPages[0] == tabPage)
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
            backRect.Height += this.Bounds.Y - 1;
            backRect.Y = 0;

            Rectangle borderRect = this.Bounds;
            borderRect.Width -= 7;
            borderRect.Height -= 28;
            borderRect.X += 3;
            borderRect.Y = 24;

            g.FillRectangle(new SolidBrush(Constants.BACKGROUND_COLOR), backRect);
            g.DrawRectangle(new Pen(Constants.TAB_BORDER_COLOR), borderRect);
        }

        public void SwitchToTab(int index)
        {
            this.SwitchToTab(this.Tabs.ElementAt(index).Value.Name);
        }

        delegate void SwitchToTabDelegate(string tabName);

        public void SwitchToTab(string tabName)
        {
            if (this.InvokeRequired)
            {
                SwitchToTabDelegate del = new SwitchToTabDelegate(this.SwitchToTab);

                this.Invoke(del, tabName);
                return;
            }

            if (this.SelectedTab.Name == tabName)
            {
                return;
            }

            this.TabPages.Add(this.Tabs[tabName]);
            this.TabPages.RemoveAt(0);
        }

        public Rectangle GetTabRectangle(int index)
        {
            Rectangle rect = new Rectangle(3, 2, 6, 21);

            for (int i = 0; i < index; i++)
            {
                Rectangle tabRect = this.GetTabRectangle(i);

                rect.X += tabRect.Width + 3;
            }

            TabPage tabPage = this.Tabs.ElementAt(index).Value;

            rect.Width += tabPage.Text.Length * 7;

            return rect;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            for (int i = 0; i < this.Tabs.Count; i++)
            {
                Rectangle rect = this.GetTabRectangle(i);

                if (rect.Contains(e.Location))
                {
                    this.SwitchToTab(i);
                    return;
                }
            }
        }
    }
}
