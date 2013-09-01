﻿using System.Windows;
using System.Windows.Controls;

namespace DesktopCS.Controls
{
    public class CSTabItem : TabItem
    {
        static CSTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CSTabItem), new FrameworkPropertyMetadata(typeof(CSTabItem)));
        }

        public static readonly DependencyProperty IsUnreadProperty = DependencyProperty.Register("IsUnread", typeof(bool), typeof(CSTabItem), new PropertyMetadata(false));
        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register("IsClosable", typeof(bool), typeof(CSTabItem), new PropertyMetadata(false));

        public static readonly RoutedEvent CloseTabEvent =
            EventManager.RegisterRoutedEvent("CloseTab", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(CSTabItem));

        public event RoutedEventHandler CloseTab
        {
            add { this.AddHandler(CloseTabEvent, value); }
            remove { this.RemoveHandler(CloseTabEvent, value); }
        }

        [System.ComponentModel.Description("Shows or hides the close button.")]
        public bool IsClosable
        {
            get { return (bool)this.GetValue(IsClosableProperty); }
            set { this.SetValue(IsClosableProperty, value); }
        }

        [System.ComponentModel.Description("Shows or hides the unread glow.")]
        public bool IsUnread
        {
            get { return (bool)this.GetValue(IsUnreadProperty); }
            set { this.SetValue(IsUnreadProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var closeButton = this.GetTemplateChild("PART_Close") as Button;
            if (closeButton != null)
                closeButton.Click += this.closeButton_Click;
        }

        void closeButton_Click(object sender, RoutedEventArgs e)
        {
            var args = new RoutedEventArgs(CloseTabEvent, this);
            this.RaiseEvent(args);
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);

            this.IsUnread = false;
        }
    }
}
