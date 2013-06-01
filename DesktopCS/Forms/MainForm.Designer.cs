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
            this.components = new System.ComponentModel.Container();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.OptionsMenuStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.TopicLabel = new System.Windows.Forms.Label();
            this.TabList = new DesktopCS.Forms.TabList();
            this.UserList = new DesktopCS.Forms.UserList();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OptionsMenuStripItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(702, 24);
            this.MenuStrip.TabIndex = 3;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // OptionsMenuStripItem
            // 
            this.OptionsMenuStripItem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptionsMenuStripItem.Name = "OptionsMenuStripItem";
            this.OptionsMenuStripItem.Size = new System.Drawing.Size(62, 20);
            this.OptionsMenuStripItem.Text = "Options";
            this.OptionsMenuStripItem.Click += new System.EventHandler(this.ToolsMenuStripItem_Click);
            // 
            // InputBox
            // 
            this.InputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InputBox.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.InputBox.Location = new System.Drawing.Point(4, 360);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(502, 13);
            this.InputBox.TabIndex = 4;
            this.InputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputBox_KeyDown);
            // 
            // TopicLabel
            // 
            this.TopicLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TopicLabel.AutoSize = true;
            this.TopicLabel.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.TopicLabel.Location = new System.Drawing.Point(3, 24);
            this.TopicLabel.MaximumSize = new System.Drawing.Size(697, 0);
            this.TopicLabel.Name = "TopicLabel";
            this.TopicLabel.Size = new System.Drawing.Size(0, 12);
            this.TopicLabel.TabIndex = 6;
            this.TopicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TopicLabel.ClientSizeChanged += new System.EventHandler(this.TopicLabel_ClientSizeChanged);
            // 
            // TabList
            // 
            this.TabList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabList.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TabList.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.TabList.Location = new System.Drawing.Point(0, 24);
            this.TabList.Name = "TabList";
            this.TabList.SelectedIndex = 0;
            this.TabList.Size = new System.Drawing.Size(510, 319);
            this.TabList.TabIndex = 5;
            this.TabList.SelectedIndexChanged += new System.EventHandler(this.TabList_SelectedIndexChanged);
            // 
            // UserList
            // 
            this.UserList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(39)))), ((int)(((byte)(46)))));
            this.UserList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UserList.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.UserList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(191)))));
            this.UserList.ImageIndex = 0;
            this.UserList.Location = new System.Drawing.Point(512, 49);
            this.UserList.Name = "UserList";
            this.UserList.SelectedImageIndex = 0;
            this.UserList.ShowPlusMinus = false;
            this.UserList.ShowRootLines = false;
            this.UserList.Size = new System.Drawing.Size(186, 312);
            this.UserList.Sorted = true;
            this.UserList.TabIndex = 1;
            this.UserList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.UserList_NodeMouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 381);
            this.Controls.Add(this.TopicLabel);
            this.Controls.Add(this.TabList);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.UserList);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = this.ClientSize;
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
        private System.Windows.Forms.ToolStripMenuItem OptionsMenuStripItem;
        private System.Windows.Forms.TextBox InputBox;
        private Forms.TabList TabList;
        private System.Windows.Forms.Label TopicLabel;
    }
}

