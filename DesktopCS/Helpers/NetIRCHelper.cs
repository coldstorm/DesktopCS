using System.Collections.Generic;
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
    }
}
