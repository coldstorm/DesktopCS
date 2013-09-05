using System;
using System.Collections.Generic;
using DesktopCS.Models;
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
            return user.ToUserItem(null);
        }

        public static UserItem ToUserItem(this User user, Channel channel)
        {
            var rank = UserRank.None;
            if (channel != null)
                rank = user.Ranks[channel.Name];

            return new UserItem(rank, user.NickName, IdentHelper.Parse(user.UserName));
        }

        public static void AddChat(this Tab tab, string text)
        {
            tab.AddChat(u => new MessageLine(u, text));
        }

        public static void AddChat(this Tab tab, User user, string text)
        {
            tab.AddChat(user, u => new MessageLine(u, text));
        }

        public static void AddChat(this Tab tab, User user, Channel channel, string text)
        {
            tab.AddChat(user, channel, u => new MessageLine(u, text));
        }

        public static void AddChat(this Tab tab, Func<UserItem, ChatLine> callback)
        {
            tab.AddChat(null, callback);
        }

        public static void AddChat(this Tab tab, User user, Func<UserItem, ChatLine> callback)
        {
            tab.AddChat(user, null, callback);
        }

        public static void AddChat(this Tab tab, User user, Channel channel, Func<UserItem, ChatLine> callback)
        {
            UserItem u = null;
            if (user != null)
                u = user.ToUserItem(channel);
            ChatLine line = callback(u); ;
            tab.AddChat(line);
        }
    }
}
