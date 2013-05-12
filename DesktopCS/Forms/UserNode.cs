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

            this.ForeColor = ColorFromUser(user);
            this.ImageKey = CountryFromUser(user);
        }

        public static Color ColorFromUser(User user)
        {
            string colorCode = user.UserName;

            if (colorCode.Length != 8)
            {
                return Constants.TEXT_COLOR;
            }

            colorCode = colorCode.Substring(0, 6);

            Color color;

            try
            {
                color = ColorTranslator.FromHtml("#" + colorCode);
            }
            catch (FormatException e)
            {
                return Constants.TEXT_COLOR;
            }

            return color;
        }

        public static string CountryFromUser(User user)
        {
            string countryCode = user.UserName.ToLower();

            if (countryCode.Length != 8)
            {
                return "qq";
            }

            string country = countryCode.Substring(6, 2);

            if (!UserList.CountryList.Contains(country))
            {
                return "qq";
            }

            return country;
        }
    }
}
