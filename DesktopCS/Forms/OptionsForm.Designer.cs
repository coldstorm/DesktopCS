namespace DesktopCS.Forms
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TitleLabel = new System.Windows.Forms.Label();
            this.NicknameLabel = new System.Windows.Forms.Label();
            this.NicknameBox = new System.Windows.Forms.TextBox();
            this.ColorBox = new System.Windows.Forms.TextBox();
            this.ColorLabel = new System.Windows.Forms.Label();
            this.ChannelsBox = new System.Windows.Forms.TextBox();
            this.ChannelsLabel = new System.Windows.Forms.Label();
            this.SoundsCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ColorChooser = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(12, 9);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(139, 18);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "General Options";
            // 
            // NicknameLabel
            // 
            this.NicknameLabel.AutoSize = true;
            this.NicknameLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NicknameLabel.Location = new System.Drawing.Point(52, 46);
            this.NicknameLabel.Name = "NicknameLabel";
            this.NicknameLabel.Size = new System.Drawing.Size(76, 14);
            this.NicknameLabel.TabIndex = 1;
            this.NicknameLabel.Text = "Nickname: ";
            // 
            // NicknameBox
            // 
            this.NicknameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(39)))), ((int)(((byte)(46)))));
            this.NicknameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NicknameBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NicknameBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(191)))));
            this.NicknameBox.Location = new System.Drawing.Point(134, 44);
            this.NicknameBox.Name = "NicknameBox";
            this.NicknameBox.Size = new System.Drawing.Size(135, 22);
            this.NicknameBox.TabIndex = 2;
            // 
            // ColorBox
            // 
            this.ColorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(39)))), ((int)(((byte)(46)))));
            this.ColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColorBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColorBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(191)))));
            this.ColorBox.Location = new System.Drawing.Point(134, 71);
            this.ColorBox.Name = "ColorBox";
            this.ColorBox.Size = new System.Drawing.Size(135, 22);
            this.ColorBox.TabIndex = 4;
            this.ColorBox.DoubleClick += new System.EventHandler(this.ColorBox_DoubleClick);
            // 
            // ColorLabel
            // 
            this.ColorLabel.AutoSize = true;
            this.ColorLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColorLabel.Location = new System.Drawing.Point(52, 73);
            this.ColorLabel.Name = "ColorLabel";
            this.ColorLabel.Size = new System.Drawing.Size(45, 14);
            this.ColorLabel.TabIndex = 3;
            this.ColorLabel.Text = "Color:";
            // 
            // ChannelsBox
            // 
            this.ChannelsBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(39)))), ((int)(((byte)(46)))));
            this.ChannelsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChannelsBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChannelsBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(191)))));
            this.ChannelsBox.Location = new System.Drawing.Point(134, 98);
            this.ChannelsBox.Name = "ChannelsBox";
            this.ChannelsBox.Size = new System.Drawing.Size(135, 22);
            this.ChannelsBox.TabIndex = 6;
            // 
            // ChannelsLabel
            // 
            this.ChannelsLabel.AutoSize = true;
            this.ChannelsLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChannelsLabel.Location = new System.Drawing.Point(52, 100);
            this.ChannelsLabel.Name = "ChannelsLabel";
            this.ChannelsLabel.Size = new System.Drawing.Size(71, 14);
            this.ChannelsLabel.TabIndex = 5;
            this.ChannelsLabel.Text = "Channels:";
            // 
            // SoundsCheckBox
            // 
            this.SoundsCheckBox.AutoSize = true;
            this.SoundsCheckBox.Checked = true;
            this.SoundsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SoundsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SoundsCheckBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SoundsCheckBox.Location = new System.Drawing.Point(55, 128);
            this.SoundsCheckBox.Name = "SoundsCheckBox";
            this.SoundsCheckBox.Size = new System.Drawing.Size(70, 18);
            this.SoundsCheckBox.TabIndex = 8;
            this.SoundsCheckBox.Text = "Sounds";
            this.SoundsCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(39)))), ((int)(((byte)(46)))));
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(251, 187);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 25);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(28)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(338, 224);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SoundsCheckBox);
            this.Controls.Add(this.ChannelsBox);
            this.Controls.Add(this.ChannelsLabel);
            this.Controls.Add(this.ColorBox);
            this.Controls.Add(this.ColorLabel);
            this.Controls.Add(this.NicknameBox);
            this.Controls.Add(this.NicknameLabel);
            this.Controls.Add(this.TitleLabel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(191)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label NicknameLabel;
        private System.Windows.Forms.TextBox NicknameBox;
        private System.Windows.Forms.TextBox ColorBox;
        private System.Windows.Forms.Label ColorLabel;
        private System.Windows.Forms.TextBox ChannelsBox;
        private System.Windows.Forms.Label ChannelsLabel;
        private System.Windows.Forms.CheckBox SoundsCheckBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ColorDialog ColorChooser;
    }
}