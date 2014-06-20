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

        public static Dictionary<UserRank, char?> RankChars = new Dictionary<UserRank, char?>
        {
            {
                UserRank.Owner,
                '@'
            },
            {
                UserRank.Admin,
                '@'
            },
            {
                UserRank.Op,
                '@'
            },
            {
                UserRank.HalfOp,
                '#'
            },
            {
                UserRank.Voice,
                '+'
            },
            {
                UserRank.None,
                null
            }
        };

        public static Dictionary<char, UserRank> ChannelRankModeChars = new Dictionary<char, UserRank>()
        {
            {
                'q',
                UserRank.Owner
            },
            {
                'a',
                UserRank.Admin
            },
            {
                'o',
                UserRank.Op
            },
            {
                'h',
                UserRank.HalfOp
            },
            {
                'v',
                UserRank.Voice
            }
        };

        public static Dictionary<char, string> ChannelModeChars = new Dictionary<char, string>()
        {
            {
                'i',
                "invite only"
            },
            {
                'm',
                "moderated"
            },
            {
                'n',
                "no outside messages"
            },
            {
                'p',
                "private"
            },
            {
                's',
                "secret"
            },
            {
                't',
                "topic-locked"
            },
        };

        public static bool IsChannel(string name)
        {
            return TypeChars.ContainsValue(name[0]);
        }
    }
}
