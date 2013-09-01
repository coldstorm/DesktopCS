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
        public MainView(SettingsManager settings, LoginData loginData)
        {
            InitializeComponent();
            DataContext = new MainViewModel(settings, loginData);
        }
    }
}
