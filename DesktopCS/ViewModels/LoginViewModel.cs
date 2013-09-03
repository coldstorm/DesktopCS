using DesktopCS.Commands;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services;
using DesktopCS.Views;

namespace DesktopCS.ViewModels
{
    class LoginViewModel : ObservableObject
    {
        private readonly LoginData _loginData;
        private bool? _dialogResult;

        public LoginCommand LoginCommand { get; private set; }

        public LoginData LoginData
        {
            get { return this._loginData; }
        }

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
            this._loginData = loginData;

            this.LoginCommand = new LoginCommand(this);
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
