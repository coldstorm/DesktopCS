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
            _vm = vm;
        }

        #region ICommand Members
        public void Execute(object parameter)
        {
            _vm.Login();
        }

        public bool CanExecute(object parameter)
        {
            return _vm.CanLogin;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
