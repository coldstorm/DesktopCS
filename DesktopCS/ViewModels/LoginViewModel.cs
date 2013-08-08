using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.Models;

namespace DesktopCS.ViewModels
{
    class LoginViewModel
    {
        private readonly LoginData _loginData;

        public LoginViewModel()
        {
            _loginData = new LoginData();
        }

        public LoginData LoginData
        {
            get { return _loginData; }
        }
        
    }
}
