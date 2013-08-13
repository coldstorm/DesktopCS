using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using NetIRC;

namespace DesktopCS.Helpers
{
    public static class NetIRCExtentions
    {

        internal static Dictionary<ChannelType, char> TypeChars = new Dictionary<ChannelType, char>()
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

        public static string FullName(this Channel channel)
        {
            return TypeChars[channel.Type] + channel.Name;
        }
    }
}
