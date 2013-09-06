using System.Windows.Input;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services;

namespace DesktopCS.ViewModels
{
    class LoginViewModel : ObservableObject
    {
        public LoginData LoginData { get; private set; }
        public ICommand LoginCommand { get; private set; }

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get { return this._dialogResult; }
            set
            {
                this._dialogResult = value;
                this.OnPropertyChanged("DialogResult");
            }
        }

        public LoginViewModel()
        {
            this.LoginData = SettingsManager.Value.GetLoginData();

            this.LoginCommand = new RelayCommand(param => Login(), param => CanLogin);
        }

        public bool CanLogin
        {
            get { return this.LoginData.IsValid; }
        }

        public void Login()
        {
            SettingsManager.Value.SetLoginData(this.LoginData);
            this.DialogResult = true;
        }
    }
}
