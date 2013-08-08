using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace DesktopCS.Models
{
    class LoginData : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _username;
        private string _password;
        private string _color;

        public string Username
        {
            get { return _username; }
            set 
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyPropertyChanged
        public string this[string columnName]
        {
            get { return GetValidationError(columnName); }
        }

        public string Error { get; private set; }
        #endregion

        #region Validation

        private string GetValidationError(string columnName)
        {
            switch (columnName)
            {
                case "Username":
                    return ValidateUsername();
                case "Color":
                    return ValidateColor();
            }
            return null;
        }

        private string ValidateUsername()
        {
            if (String.IsNullOrWhiteSpace(Username))
                return "Username cannot be empty.";

            return null;
        }

        private string ValidateColor()
        {
            var regex = new Regex(@"[0-9a-fA-F]{6}");
            if (Color != null && !regex.IsMatch(Color)) 
                return "Invalid color.";

            return null;
        }

        #endregion
    }
}
