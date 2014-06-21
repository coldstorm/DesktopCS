using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.Models
{
    public class WindowGeometry
    {
        public double Top;
        public double Left;
        public double Height;
        public double Width;
        public bool IsMaximized;

        public WindowGeometry(double top, double left, double height, double width, bool isMaximized)
        {
            this.Top = top;
            this.Left = left;
            this.Height = height;
            this.Width = width;
            this.IsMaximized = isMaximized;
        }
    }
}
