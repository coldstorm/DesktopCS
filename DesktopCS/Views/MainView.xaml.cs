using DesktopCS.Models;
using DesktopCS.Services;
using DesktopCS.ViewModels;

namespace DesktopCS.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView(LoginData loginData)
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel(loginData);
        } 
        
        //Keep the focus on InputTextBox all the time
        private void PreviewWindow_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.InputTextBox.Focus();
        }
    }
}
