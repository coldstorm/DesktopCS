using System;
using System.Collections.Generic;
using DesktopCS.Models;
using DesktopCS.Services;
using NetIRC;

namespace DesktopCS.Helpers
{
    public static class NetIRCHelper
    {
        internal static Dictionary<ChannelType, char> TypeChars = new Dictionary<ChannelType, char>
            {
                {
                    ChannelType.Network,
                    '#'
                },
                {
                    ChannelType.Local,
                    '&'
                },
                {
                    ChannelType.Safe,
                    '!'
                },
                {
                    ChannelType.Unmoderated,
                    '+'
                }
            };

        public static UserItem ToUserItem(this User user)
        {
            return new UserItem(user.NickName, IdentHelper.Parse(user.UserName));
        }

        public static void AddChat(this Tab tab, User user, Func<UserItem, ChatLine> callback)
        {
            UserItem u = null;
            if (user != null)
                u = user.ToUserItem();
            ChatLine line = callback(u); ;
            tab.AddChat(line);
        }
    }
}
