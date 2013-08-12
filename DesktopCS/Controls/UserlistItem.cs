using System.Windows;
using System.Windows.Controls;

namespace DesktopCS.Controls
{
    public class UserlistItem : ContentControl
    {
        static UserlistItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserlistItem), new FrameworkPropertyMetadata(typeof(UserlistItem)));
        }

        public static DependencyProperty FlagProperty = DependencyProperty.Register("Flag", typeof(Image), typeof(UserlistItem));

        public Image Flag
        {
            get { return (Image)GetValue(FlagProperty); }
            set { SetValue(FlagProperty, value); }
        }


        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(UserlistItem));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }


        public static readonly RoutedEvent QueryEvent =
            EventManager.RegisterRoutedEvent("Query", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserlistItem));


        public event RoutedEventHandler Query
        {
            add { AddHandler(QueryEvent, value); }
            remove { RemoveHandler(QueryEvent, value); }
        }

        public static readonly RoutedEvent KickEvent =
            EventManager.RegisterRoutedEvent("Kick", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserlistItem));


        public event RoutedEventHandler Kick
        {
            add { AddHandler(KickEvent, value); }
            remove { RemoveHandler(KickEvent, value); }
        }

        public static readonly RoutedEvent BanEvent =
            EventManager.RegisterRoutedEvent("Ban", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserlistItem));


        public event RoutedEventHandler Ban
        {
            add { AddHandler(BanEvent, value); }
            remove { RemoveHandler(BanEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var queryButton = GetTemplateChild("PART_Query") as Button;
            if (queryButton != null)
                queryButton.Click += queryButton_Click;

            var kickButton = GetTemplateChild("PART_Kick") as Button;
            if (kickButton != null)
                kickButton.Click += kickButton_Click;

            var banButton = GetTemplateChild("PART_Ban") as Button;
            if (banButton != null)
                banButton.Click += banButton_Click;         
        }

        void queryButton_Click(object sender, RoutedEventArgs e)
        {
           RaiseEvent(new RoutedEventArgs(QueryEvent, this));
        }

        void kickButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(KickEvent, this));
        }

        void banButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BanEvent, this));
        }
    }
}
