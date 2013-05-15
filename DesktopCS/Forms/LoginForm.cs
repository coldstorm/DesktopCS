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
            this.ColorBox.BackColor = ColorTranslator.FromHtml(this.ColorBox.Text);

            this.FormClosing += LoginForm_FormClosing;
        }

        protected void ColorBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ColorChooser.Color = ColorTranslator.FromHtml(this.ColorBox.Text);
            }
            catch (ArgumentException ex)
            {
                ColorChooser.Color = Constants.TEXT_COLOR;
            }
            DialogResult result = ColorChooser.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string colorHex = "#" + string.Format("{0:X6}", ColorChooser.Color.ToArgb() & 0xFFFFFF);

                this.ColorBox.Text = colorHex;
                this.ColorBox.BackColor = ColorChooser.Color;
                this.ColorBox.BackColor = ColorTranslator.FromHtml(colorHex);
            }
        }

        protected void ColorBox_LostFocus(object sender, System.EventArgs e)
        {
            try
            {
                Color bgColor = ColorTranslator.FromHtml(this.ColorBox.Text);
                this.ColorBox.BackColor = bgColor;
            }
            catch (ArgumentException ex)
            {

            }
        }

        protected void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fullClose)
            {
                Application.Exit();
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Color = this.ColorBox.Text;
            Properties.Settings.Default.Nickname = this.NicknameBox.Text;

            Properties.Settings.Default.Save();

            this.fullClose = false;
            this.Close();

            MainForm main = new MainForm();
            main.Show();
        }
    }
}
