using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NetIRC;

namespace DesktopCS.Forms
{
    class UserNode : TreeNode
    {
        public User User;

        private Dictionary<UserRank, char?> RankChars = new Dictionary<UserRank, char?>()
        {
            {UserRank.None, null},
            {UserRank.Voice, '+'},
            {UserRank.HalfOp, '#'},
            {UserRank.Op, '@'},
            {UserRank.Admin, '@'},
            {UserRank.Owner, '@'}
        };

        public UserNode(User user) : base()
        {
            this.Text = this.RankChars[user.Rank] + user.NickName;
            this.User = user;

            this.ForeColor = this.ColorFromUser(user);
        }

        private Color ColorFromUser(User user)
        {
            string colorCode = User.UserName;

            if (colorCode.Length != 8)
            {
                return this.ForeColor;
            }

            colorCode = colorCode.Substring(0, 6);

            try
            {
                return ColorTranslator.FromHtml("#" + colorCode);
            }
            catch (FormatException e)
            {
                return this.ForeColor;
            }
        }
    }
}
