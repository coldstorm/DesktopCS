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
        public MainView()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();

            // Load up window geometry from settings
            WindowGeometry windowGeometry = SettingsManager.Value.GetWindowGeometry();
            this.Top = windowGeometry.Top;
            this.Left = windowGeometry.Left;
            this.Height = windowGeometry.Height;
            this.Width = windowGeometry.Width;

            if (windowGeometry.IsMaximized)
                this.WindowState = System.Windows.WindowState.Maximized;
        } 

        private void PreviewWindow_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Only focus on InputTextBox if there is no text selected, to allow events to be handled in ChatRichTextBox
            MainViewModel vm = this.DataContext as MainViewModel;
            if (vm.SelectedItem.Selection.IsEmpty)
            {
                this.InputTextBox.Focus();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Save window geometry for next session
            WindowGeometry windowGeometry;

            if (this.WindowState == System.Windows.WindowState.Maximized)
                windowGeometry = new WindowGeometry(this.RestoreBounds.Top, this.RestoreBounds.Left, this.RestoreBounds.Height, this.RestoreBounds.Width, true);
            else
                windowGeometry = new WindowGeometry(this.Top, this.Left, this.Height, this.Width, false);

            SettingsManager.Value.SetWindowGeometry(windowGeometry);
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.InputTextBox.Focus();
        }
    }
}
