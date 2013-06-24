using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopCS.Forms
{
    public partial class YouTubeForm : Form
    {
        public YouTubeForm(string videoID)
        {
            InitializeComponent();

            BackColor = Constants.BACKGROUND_COLOR;
            ForeColor = Constants.TEXT_COLOR;

            this.Browser.DocumentText = "<html><body></body></html>";

            this.Browser.Document.ForeColor = Constants.TEXT_COLOR;
            this.Browser.Document.BackColor = Constants.CHAT_BACKGROUND_COLOR;

            HtmlElement youtubeElement = this.Browser.Document.CreateElement("iframe");
            youtubeElement.SetAttribute("width", "100%");
            youtubeElement.SetAttribute("height", "100%");
            youtubeElement.SetAttribute("src", String.Format("https://www.youtube.com/embed/{0}", videoID));
            youtubeElement.SetAttribute("frameborder", "0");
            youtubeElement.SetAttribute("allowfullscreen", "allowfullscreen");

            while (this.Browser.Document.Body == null)
            {
                Application.DoEvents();
            }

            this.Browser.Document.Body.AppendChild(youtubeElement);


        }
    }
}
