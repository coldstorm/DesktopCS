using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using DesktopCS.Models;

namespace DesktopCS.Tabs
{
    class MessageLine : ChatLine
    {
        public MessageLine(UserListItem user, string message) : base(user, (SolidColorBrush) Application.Current.FindResource("ChatBrush"), message)
        {
            
        }
    }
}
