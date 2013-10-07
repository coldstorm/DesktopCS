using System;
using DesktopCS.Helpers.Parsers;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.Helpers.Extentions
{
    public static class NetIRCHelper
    {
        public static UserItem ToUserItem(this User user)
        {
            return user.ToUserItem(null);
        }

        public static UserItem ToUserItem(this User user, Channel channel)
        {
            var rank = UserRank.None;
            if (channel != null)
                if (user.Ranks.ContainsKey(channel.Name))
                    rank = user.Ranks[channel.Name];

            return new UserItem(rank, user.NickName, IdentHelper.Parse(user.UserName));
        }

        public static void AddChat(this Tab tab, string text, ParseArgs args)
        {
            tab.AddChat(u => new MessageLine(u, text, args));
        }

        public static void AddChat(this Tab tab, User user, string text, ParseArgs args)
        {
            tab.AddChat(user, u => new MessageLine(u, text, args));
        }

        public static void AddChat(this Tab tab, User user, Channel channel, string text, ParseArgs args)
        {
            tab.AddChat(user, channel, u => new MessageLine(u, text, args));
        }

        public static void AddChat(this Tab tab, Func<UserItem, ChatLine> callback)
        {
            AddChat(tab, null, callback);
        }

        public static void AddChat(this Tab tab, User user, Func<UserItem, ChatLine> callback)
        {
            AddChat(tab, user, null, callback);
        }

        public static void AddChat(this Tab tab, User user, Channel channel, Func<UserItem, ChatLine> callback)
        {
            UserItem u = null;
            if (user != null)
                u = user.ToUserItem(channel);
            ChatLine line = callback(u);
            tab.AddChat(line);
        }
    }
}
