using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.Helpers;

namespace DesktopCS.Models
{
    public class ErrorLine : ChatLine
    {
        public ErrorLine(string error)
            : base(ColorHelper.WarningColor, error)
        {
            
        }
    }
}
