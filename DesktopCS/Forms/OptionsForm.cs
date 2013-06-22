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
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();

            this.NicknameBox.Text = Properties.Settings.Default.Nickname;
            this.ColorBox.Text = Properties.Settings.Default.Color;
            this.ColorBox.BackColor = ColorTranslator.FromHtml(this.ColorBox.Text);
            this.ChannelsBox.Text = Properties.Settings.Default.Channels;
            this.SoundsCheckBox.Checked = Properties.Settings.Default.Sounds;
            this.SpoilersCheckBox.Checked = Properties.Settings.Default.Spoilers;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Nickname = this.NicknameBox.Text;
            Properties.Settings.Default.Color = this.ColorBox.Text;
            Properties.Settings.Default.Channels = this.ChannelsBox.Text;
            Properties.Settings.Default.Sounds = this.SoundsCheckBox.Checked;
            Properties.Settings.Default.Spoilers = this.SpoilersCheckBox.Checked;

            Properties.Settings.Default.Save();
            this.Close();
        }

        private void ColorBox_DoubleClick(object sender, EventArgs e)
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
    }
}
