using System;
using System.Windows.Input;
using DesktopCS.ViewModels;

namespace DesktopCS.Commands
{
    class LoginCommand : ICommand
    {
        private readonly LoginViewModel _vm;

        public LoginCommand(LoginViewModel vm)
        {
            this._vm = vm;
        }

        #region ICommand Members
        public void Execute(object parameter)
        {
            this._vm.Login();
        }

        public bool CanExecute(object parameter)
        {
            return this._vm.CanLogin;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
