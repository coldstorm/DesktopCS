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
            cTabControl.TabPages.Add(Tab);
        }

        private void RemoveTab(int index)
        {
            cTabControl.TabPages.RemoveAt(index);
        }
    }
}
