using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DesktopCS.Models
{
    class SystemMessageLine : ChatLine
    {
        public SystemMessageLine(string message) : base((SolidColorBrush) Application.Current.FindResource("MessageBrush"), message)
        {
            
        }
    }
}
