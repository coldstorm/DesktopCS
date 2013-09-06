using System;
using System.Windows.Threading;
using DesktopCS.Helpers;
using DesktopCS.Models;
using DesktopCS.Services;
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
            this.InitializeComponent();
            this.DataContext = new LoginViewModel();
            
            this._timeTimer.Tick += this.timeTimer_Tick;
            this._timeTimer.Start();
        }

        private void timeTimer_Tick(object sender, EventArgs e)
        {
            this._timeTimer.Interval = new TimeSpan(0, 0, 60 - DateTime.Now.Second);
            this.Time.Text = TimeHelper.CreateTimeStamp();
        }
    }
}
