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
            get { return (Image)this.GetValue(FlagProperty); }
            set { this.SetValue(FlagProperty, value); }
        }


        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(UserlistItem));

        public bool IsExpanded
        {
            get { return (bool)this.GetValue(IsExpandedProperty); }
            set { this.SetValue(IsExpandedProperty, value); }
        }


        public static readonly RoutedEvent QueryEvent =
            EventManager.RegisterRoutedEvent("Query", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserlistItem));


        public event RoutedEventHandler Query
        {
            add { this.AddHandler(QueryEvent, value); }
            remove { this.RemoveHandler(QueryEvent, value); }
        }

        public static readonly RoutedEvent KickEvent =
            EventManager.RegisterRoutedEvent("Kick", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserlistItem));


        public event RoutedEventHandler Kick
        {
            add { this.AddHandler(KickEvent, value); }
            remove { this.RemoveHandler(KickEvent, value); }
        }

        public static readonly RoutedEvent BanEvent =
            EventManager.RegisterRoutedEvent("Ban", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserlistItem));


        public event RoutedEventHandler Ban
        {
            add { this.AddHandler(BanEvent, value); }
            remove { this.RemoveHandler(BanEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var queryButton = this.GetTemplateChild("PART_Query") as Button;
            if (queryButton != null)
                queryButton.Click += this.queryButton_Click;

            var kickButton = this.GetTemplateChild("PART_Kick") as Button;
            if (kickButton != null)
                kickButton.Click += this.kickButton_Click;

            var banButton = this.GetTemplateChild("PART_Ban") as Button;
            if (banButton != null)
                banButton.Click += this.banButton_Click;         
        }

        void queryButton_Click(object sender, RoutedEventArgs e)
        {
           this.RaiseEvent(new RoutedEventArgs(QueryEvent, this));
        }

        void kickButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(KickEvent, this));
        }

        void banButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(BanEvent, this));
        }
    }
}
