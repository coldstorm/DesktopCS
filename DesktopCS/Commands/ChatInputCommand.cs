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
            _vm = vm;
        }

        #region ICommand Members
        public void Execute(object parameter)
        {
            _vm.Chat(parameter as string);
        }

        public bool CanExecute(object parameter)
        {
            return _vm.CanChat;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
