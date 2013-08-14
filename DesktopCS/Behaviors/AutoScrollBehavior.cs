using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DesktopCS.Behaviors
{

    public class AutoScrollBehavior : Behavior<ScrollViewer>
    {
        private ScrollViewer _scrollViewer;

        protected override void OnAttached()
        {
            base.OnAttached();

            _scrollViewer = AssociatedObject;
            _scrollViewer.LayoutUpdated += _scrollViewer_LayoutUpdated;
        }

        private void _scrollViewer_LayoutUpdated(object sender, EventArgs e)
        {
            if (_scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight)
                _scrollViewer.ScrollToBottom();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_scrollViewer != null)
                _scrollViewer.LayoutUpdated -= _scrollViewer_LayoutUpdated;
        }
    }
}
