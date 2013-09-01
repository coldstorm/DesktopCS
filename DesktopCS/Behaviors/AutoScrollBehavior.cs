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

            this._scrollViewer = this.AssociatedObject;
            this._scrollViewer.LayoutUpdated += this._scrollViewer_LayoutUpdated;
        }

        private void _scrollViewer_LayoutUpdated(object sender, EventArgs e)
        {
            if ((int)this._scrollViewer.VerticalOffset == (int)this._scrollViewer.ScrollableHeight)
                this._scrollViewer.ScrollToBottom();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this._scrollViewer != null)
                this._scrollViewer.LayoutUpdated -= this._scrollViewer_LayoutUpdated;
        }
    }
}
