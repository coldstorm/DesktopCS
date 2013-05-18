using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using NetIRC;

namespace DesktopCS.Forms
{
    [System.ComponentModel.DesignerCategory("")]
    class BaseTab : TabPage
    {
        public TabType Type;

        public WebBrowser Browser;

        public bool Active = false;

        public BaseTab(string title)
        {
            this.Name = title;
            this.Text = title;

            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            Browser = new WebBrowser();
            Browser.Dock = DockStyle.Fill;
            Browser.Name = "Browser";

            Browser.DocumentText = "<html><body></body></html>";

            while (Browser.Document.Body == null)
            {
                Application.DoEvents();
            }

            Browser.Document.ForeColor = Constants.TEXT_COLOR;
            Browser.Document.BackColor = Constants.CHAT_BACKGROUND_COLOR;

            Browser.Document.Body.Style = "font-size:10px;font-family:verdana;margin:0;padding:0;";

            Browser.Navigating += (s, e) =>
            {
                e.Cancel = true;

                switch (e.Url.Scheme)
                {
                    case "cs-pm":
                        MainForm mainForm = this.Parent.Parent as MainForm;

                        string userName = e.Url.LocalPath;
                        User user = null;

                        foreach (Channel channel in mainForm.Client.Channels.Values)
                        {
                            if (channel.Users.ContainsKey(userName))
                            {
                                user = channel.Users[userName];
                                break;
                            }
                        }

                        if (user == null)
                        {
                            break;
                        }

                        PrivateMessageTab tab = new PrivateMessageTab(user);

                        mainForm.AddTab(tab);

                        break;

                    case "cs-channel":
                        mainForm = this.Parent.Parent as MainForm;

                        mainForm.Client.JoinChannel(e.Url.LocalPath);

                        break;

                    default:
                        Process.Start(e.Url.AbsoluteUri);

                        break;
                }
            };

            this.Controls.Add(Browser);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.FillRectangle(new SolidBrush(Constants.CHAT_BACKGROUND_COLOR), this.DisplayRectangle);
        }
    }
}
