using System;
using System.Windows.Documents;
using System.Windows.Media;
using DesktopCS.Controls;
using DesktopCS.Helpers;
using DesktopCS.Models;

namespace DesktopCS.UserControls
{
    /// <summary>
    /// Interaction logic for TabUserControl.xaml
    /// </summary>
    public partial class TabUserControl
    {
        private readonly CSTabItem _parent;

        public TabUserControl(CSTabItem parent)
        {
            _parent = parent;
            InitializeComponent();
        }


    }
}
