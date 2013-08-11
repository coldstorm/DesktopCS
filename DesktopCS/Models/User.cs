using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.Models
{
    class User
    {
        public User(string rank, string username, string realName, string host, string metadata)
        {
            Host = host;
            RealName = realName;
            Metadata = metadata;
            Username = username;
            Rank = rank;
        }

        public string Rank { get; private set; }
        public string Username { get; private set; }
        public string RealName { get; private set; }
        public string Host { get; private set; }
        public string Metadata { get; private set; }
    }
}
