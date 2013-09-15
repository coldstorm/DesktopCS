using System.Collections.Generic;
using NetIRC;

namespace DesktopCS.Helpers
{
    public static class NetIRCHelper
    {
        public static Dictionary<ChannelType, char> TypeChars = new Dictionary<ChannelType, char>
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

        public static Dictionary<UserRank, char> RankChars = new Dictionary<UserRank, char>
        {
            {
                UserRank.Owner,
                '~'
            },
            {
                UserRank.Admin,
                '&'
            },
            {
                UserRank.Op,
                '@'
            },
            {
                UserRank.HalfOp,
                '#' // CS specific
            },
            {
                UserRank.Voice,
                '+'
            }
        };

        public static bool IsChannel(string name)
        {
            return TypeChars.ContainsValue(name[0]);
        }
    }
}
