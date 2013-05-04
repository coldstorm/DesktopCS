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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            AddTab("test");
            AddTab("test2");

            AddLine(0, "test");
            AddLine(1, "test");
            AddLine(0, "test");

            BackColor = Constants.BACKGROUND_COLOR;
            ForeColor = Constants.TEXT_COLOR;
            MainMenuStrip.BackColor = BackColor;
            MainMenuStrip.ForeColor = ForeColor;
            Userlist.BackColor = BackColor;
            Userlist.ForeColor = ForeColor;
            InputBox.BackColor = BackColor;
            InputBox.ForeColor = ForeColor;
        }

        private void AddTab(string title)
        {
            CTabPage Tab = new CTabPage(title);

            //Prepare RichTextBox
            RichTextBox TextBox = new RichTextBox();
            TextBox.Name = "TextBox";
            TextBox.Dock = DockStyle.Fill;
            TextBox.BorderStyle = BorderStyle.None;
            TextBox.BackColor = Constants.CHAT_BACKGROUND_COLOR;
            TextBox.ForeColor = Constants.TEXT_COLOR;
            TextBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TextBox.ReadOnly = true;

            Tab.Controls.Add(TextBox);
            cTabControl.TabPages.Add(Tab);
        }

        private void RemoveTab(int index)
        {
            cTabControl.TabPages.RemoveAt(index);
        }

        private void AddLine(int tabIndex, string line)
        {
            line = line.Trim();
            string formattedline = DateTime.Now.ToString("[HH:mm] ") + line + "\n";
            cTabControl.TabPages[tabIndex].Controls["TextBox"].Text += formattedline;
        }
    }
}
