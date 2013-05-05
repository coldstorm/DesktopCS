using System;
using System.Windows.Forms;

namespace DesktopCS.Forms
{
    class UserList : TreeView
    {
        public UserList() : base()
        {
            this.BackColor = Constants.CHAT_BACKGROUND_COLOR;
            this.ForeColor = Constants.TEXT_COLOR;

            this.BorderStyle = BorderStyle.None;

            this.SelectedNode = null;
            this.ShowRootLines = false;
            this.ShowPlusMinus = false;
        }
    }
}
