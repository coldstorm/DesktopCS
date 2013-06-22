using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using DesktopCS.Properties;
using NetIRC;

namespace DesktopCS.Forms
{
    [System.ComponentModel.DesignerCategory("")]
    class UserList : TreeView
    {
        public static List<string> CountryList = new List<string>();

        public UserList() : base()
        {
            this.BackColor = Constants.CHAT_BACKGROUND_COLOR;
            this.ForeColor = Constants.TEXT_COLOR;

            this.BorderStyle = BorderStyle.None;

            this.SelectedNode = null;
            this.ShowRootLines = false;
            this.ShowPlusMinus = false;

            this.TreeViewNodeSorter = new UserNodeSorter();

            this.ImageList = new ImageList();
            this.ImageList.ImageSize = new Size(16, 11);

            ResourceManager manager = Resources.ResourceManager;
            ResourceSet resources = manager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            foreach (DictionaryEntry pair in resources)
            {
                if (!pair.Key.ToString().StartsWith("icon_"))
                {
                    continue;
                }

                string country = pair.Key.ToString().Substring(5).Split('.')[0];
                Bitmap icon = (Bitmap)manager.GetObject(pair.Key.ToString());

                this.ImageList.Images.Add(country, icon);

                if (!UserList.CountryList.Contains(country))
                {
                    UserList.CountryList.Add(country);
                }
            }
        }

        public void PopulateFromChannel(Channel channel)
        {
            this.Nodes.Clear();

            foreach (User user in channel.Users.Values)
            {
                this.Nodes.Add(new UserNode(user));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Logger.Log("UserList.OnResize was called.");
            Logger.Log("[UserList.OnResize] UserList size: " + this.Width + "x" + this.Height);
        }
    }
}
