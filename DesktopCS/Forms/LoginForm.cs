using System;
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

        protected void LoginForm_Load(object sender, System.EventArgs e)
        {
            this.Close();

            MainForm main = new MainForm();
            main.Show();
        }
    }
}
