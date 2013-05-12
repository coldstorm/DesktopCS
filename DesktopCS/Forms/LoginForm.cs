using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopCS.Forms
{
    public partial class LoginForm : Form
    {
        private bool fullClose = true;

        public LoginForm()
        {
            InitializeComponent();

            this.NicknameBox.Text = Properties.Settings.Default.Nickname;
            this.ColorBox.Text = Properties.Settings.Default.Color;

            //this.Load += LoginForm_Load;
            this.FormClosing += LoginForm_FormClosing;
        }

        protected void ColorBox_MouseDoubleClick(object sender, MouseEventArgs e)
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

        protected void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fullClose)
            {
                Application.Exit();
            }
        }

        protected void LoginForm_Load(object sender, System.EventArgs e)
        {
            this.fullClose = false;
            this.Close();

            MainForm main = new MainForm();
            main.Show();
        }
    }
}
