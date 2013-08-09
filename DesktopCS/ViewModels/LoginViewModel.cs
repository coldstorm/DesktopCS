using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DesktopCS.Commands;
using DesktopCS.Models;

namespace DesktopCS.ViewModels
{
    class LoginViewModel
    {
        private readonly LoginData _loginData;

        public LoginViewModel()
        {
            _loginData = new LoginData();

            LoginCommand = new LoginCommand(this);
        }

        public LoginCommand LoginCommand { get; private set; }

        public LoginData LoginData
        {
            get { return _loginData; }
        }

        public bool CanLogin
        {
            get { return LoginData.IsValid; }
        }

        public void Login()
        {
            throw new NotImplementedException();
        }
    }
}
