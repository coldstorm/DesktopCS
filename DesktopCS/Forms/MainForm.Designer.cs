namespace DesktopCS.Forms
{
    partial class MainForm
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
            this.Userlist = new System.Windows.Forms.TreeView();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.cTabControl = new DesktopCS.CTabControl();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Userlist
            // 
            this.Userlist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Userlist.Dock = System.Windows.Forms.DockStyle.Right;
            this.Userlist.Location = new System.Drawing.Point(581, 24);
            this.Userlist.Name = "Userlist";
            this.Userlist.Size = new System.Drawing.Size(121, 357);
            this.Userlist.TabIndex = 1;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(702, 24);
            this.MenuStrip.TabIndex = 3;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // InputBox
            // 
            this.InputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InputBox.Location = new System.Drawing.Point(0, 361);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(581, 20);
            this.InputBox.TabIndex = 4;
            // 
            // cTabControl
            // 
            this.cTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.cTabControl.Location = new System.Drawing.Point(0, 24);
            this.cTabControl.Name = "cTabControl";
            this.cTabControl.SelectedIndex = 0;
            this.cTabControl.Size = new System.Drawing.Size(581, 337);
            this.cTabControl.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 381);
            this.Controls.Add(this.cTabControl);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.Userlist);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "MainForm";
            this.Text = "Coldstorm";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView Userlist;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.TextBox InputBox;
        private CTabControl cTabControl;
    }
}

