using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetIRC;

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
            Logger.Log("TabList.AddTab was called.");

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
            Logger.Log("TabList.RemoveTab was called.");

            if (this.Tabs.Count > 0 && this.SelectedTab == tab)
            {
                this.SwitchToTab(0);
            }

            this.Tabs.Remove(tab.Name);

            if (this.Tabs.Count == 0)
            {
                this.TabPages.Remove(tab);
            }

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

            if (tabPage.Active)
            {
                textColor = Constants.ACTIVE_TAB_TEXT_COLOR;
            }

            g.DrawRectangle(new Pen(borderColor), borderRect);
            g.FillRectangle(new SolidBrush(bgColor), innerRect);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            Rectangle nameRect = borderRect;
            nameRect.Width -= 10;

            g.DrawString(tabPage.Text, new Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Pixel, 0), new SolidBrush(textColor), nameRect, format);

            Rectangle xRect = nameRect;
            xRect.X += xRect.Width;
            xRect.Width = 10;

            g.DrawString("X", new Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Pixel, 0), new SolidBrush(textColor), xRect, format);
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
            Logger.Log("TabList.SwitchToTab was called.");

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

            this.Tabs[tabName].Active = false;

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
            rect.Width += 10;

            return rect;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            for (int i = 0; i < this.Tabs.Count; i++)
            {
                Rectangle rect = this.GetTabRectangle(i);

                if (rect.Contains(e.Location))
                {
                    this.Cursor = Cursors.Hand;
                    return;
                }
            }

            this.Cursor = Cursors.Default;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            for (int i = 0; i < this.Tabs.Count; i++)
            {
                Rectangle rect = this.GetTabRectangle(i);

                if (rect.Contains(e.Location))
                {
                    Rectangle xRect = rect;
                    xRect.X += xRect.Width - 10;
                    xRect.Width = 10;

                    if (xRect.Contains(e.Location))
                    {
                        BaseTab tab = this.Tabs.Values.ToArray()[i];
                        if (tab.Type == TabType.Channel)
                        {
                            Channel channel = (tab as ChannelTab).Channel;

                            MainForm mainForm = this.Parent as MainForm;
                            mainForm.Client.LeaveChannel(channel.Name);
                        }
                        else
                        {
                            this.RemoveTab(tab);
                        }

                        return;
                    }

                    this.SwitchToTab(i);
                    return;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Logger.Log("TabList.OnResize was called.");
            base.OnResize(e);
        }
    }
}
