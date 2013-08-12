using System;
using System.Windows.Threading;
using DesktopCS.Helpers;
using DesktopCS.ViewModels;

namespace DesktopCS.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView
    {
        private readonly DispatcherTimer _timeTimer = new DispatcherTimer();

        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            
            _timeTimer.Tick += timeTimer_Tick;
            _timeTimer.Start();
        }

        private void timeTimer_Tick(object sender, EventArgs e)
        {
            _timeTimer.Interval = new TimeSpan(0, 0, 60 - DateTime.Now.Second);
            Time.Text = TimeHelper.CreateTimeStamp();
        }
    }
}
