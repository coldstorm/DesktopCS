using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DesktopCS.Models
{
    class MessageLine : ChatLine
    {
        public MessageLine(User user, string message) : base(user, (SolidColorBrush) Application.Current.FindResource("ChatBrush"), message)
        {
            
        }
    }
}
