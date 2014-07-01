using System.Windows;
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
        public static readonly DependencyProperty IsConnectedProperty = DependencyProperty.Register("IsConnected", typeof(bool), typeof(CSTabItem), new PropertyMetadata(false));
        public static readonly DependencyProperty IsChannelProperty = DependencyProperty.Register("IsChannel", typeof(bool), typeof(CSTabItem), new PropertyMetadata(false));

        public static readonly RoutedEvent CloseTabEvent =
            EventManager.RegisterRoutedEvent("CloseTab", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(CSTabItem));

        public event RoutedEventHandler CloseTab
        {
            add { this.AddHandler(CloseTabEvent, value); }
            remove { this.RemoveHandler(CloseTabEvent, value); }
        }

        public static readonly RoutedEvent PartTabEvent = 
            EventManager.RegisterRoutedEvent("PartTab", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(CSTabItem));

        public event RoutedEventHandler PartTab
        {
            add { this.AddHandler(PartTabEvent, value); }
            remove { this.RemoveHandler(PartTabEvent, value); }
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

        public bool IsConnected
        {
            get { return (bool)this.GetValue(IsConnectedProperty); }
            set { this.SetValue(IsConnectedProperty, value); }
        }

        public bool IsChannel
        {
            get { return (bool)this.GetValue(IsChannelProperty); }
            set { this.SetValue(IsChannelProperty, value); }
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
            RoutedEventArgs args = null;
            if (this.IsChannel && this.IsConnected)
            {
                args = new RoutedEventArgs(PartTabEvent, this);
            }

            else
            {
                args = new RoutedEventArgs(CloseTabEvent, this);
            }
            this.RaiseEvent(args);
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);

            this.IsUnread = false;
        }
    }
}
