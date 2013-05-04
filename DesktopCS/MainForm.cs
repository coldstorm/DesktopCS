using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetIRC;

namespace DesktopCS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            AddTab("test");
            AddTab("test2");
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
