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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            this.NicknameBox.Text = Properties.Settings.Default.Nickname;
            this.ColorBox.Text = Properties.Settings.Default.Color;
        }

        private void ColorBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ColorChooser.Color = ColorTranslator.FromHtml(this.ColorBox.Text);
            DialogResult result = ColorChooser.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.ColorBox.Text = ColorTranslator.ToHtml(ColorChooser.Color);
                Properties.Settings.Default.Color = ColorTranslator.ToHtml(ColorChooser.Color);
                Properties.Settings.Default.Save();
            }
        }
    }
}
