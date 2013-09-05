using System.Windows.Input;
using DesktopCS.Models;
using DesktopCS.MVVM;

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

        public LoginViewModel(LoginData loginData)
        {
            this.LoginData = loginData;

            this.LoginCommand = new RelayCommand(param => Login(), param => CanLogin);
        }

        public bool CanLogin
        {
            get { return this.LoginData.IsValid; }
        }

        public void Login()
        {
            this.DialogResult = true;
        }
    }
}
