using DesktopCS.ViewModels;

namespace DesktopCS.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView(IRCClient irc)
        {
            InitializeComponent();
            DataContext = new MainViewModel(irc);
        }
    }
}
