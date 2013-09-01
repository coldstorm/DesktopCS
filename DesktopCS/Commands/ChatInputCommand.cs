using System;
using System.Windows.Input;
using DesktopCS.ViewModels;

namespace DesktopCS.Commands
{
    class ChatInputCommand : ICommand
    {
        private readonly MainViewModel _vm;

        public ChatInputCommand(MainViewModel vm)
        {
            this._vm = vm;
        }

        #region ICommand Members
        public void Execute(object parameter)
        {
            this._vm.Chat();
        }

        public bool CanExecute(object parameter)
        {
            return this._vm.CanChat;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
