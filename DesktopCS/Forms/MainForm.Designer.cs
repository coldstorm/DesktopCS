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
            this.UserList = new Forms.UserList();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.TabList = new DesktopCS.Forms.TabList();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserList
            // 
            this.UserList.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
            this.UserList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UserList.Location = new System.Drawing.Point(581, 50);
            this.UserList.Name = "UserList";
            this.UserList.Size = new System.Drawing.Size(117, 327);
            this.UserList.TabIndex = 1;
            this.UserList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.UserList_NodeMouseDoubleClick);
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
            this.optionsToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.optionsToolStripMenuItem.Text = "Tools";
            // 
            // InputBox
            // 
            this.InputBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.InputBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InputBox.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InputBox.Location = new System.Drawing.Point(4, 360);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(574, 21);
            this.InputBox.TabIndex = 4;
            // 
            // TabList
            // 
            this.TabList.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.TabList.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TabList.Location = new System.Drawing.Point(0, 24);
            this.TabList.Name = "cTabControl";
            this.TabList.SelectedIndex = 0;
            this.TabList.Size = new System.Drawing.Size(581, 336);
            this.TabList.TabIndex = 5;
            this.TabList.SelectedIndexChanged += new System.EventHandler(this.TabList_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 381);
            this.Controls.Add(this.TabList);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.UserList);
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

        private Forms.UserList UserList;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.TextBox InputBox;
        private Forms.TabList TabList;
    }
}

