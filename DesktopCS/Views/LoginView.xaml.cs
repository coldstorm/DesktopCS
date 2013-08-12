using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
