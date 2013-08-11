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
        private string _colorString;
        private SolidColorBrush _colorBrush;

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

        public string ColorString
        {
            get { return _colorString; }
            set
            {
                _colorString = value;
                OnPropertyChanged("ColorString");

                if (ValidateColorString() == null)
                    ColorBrush = (SolidColorBrush)new BrushConverter().ConvertFrom(ColorString);
                else
                    ColorBrush = null;
            }
        }

        public SolidColorBrush ColorBrush
        {
            get { return _colorBrush; }
            set
            {
                _colorBrush = value;
                OnPropertyChanged("ColorBrush");
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

        public bool IsValid
        {
            get { return _validatedProperties.All(property => GetValidationError(property) == null); }
        }

        private readonly string[] _validatedProperties =
            {
                "Username",
                "ColorString"
            };

        private string GetValidationError(string columnName)
        {
            switch (columnName)
            {
                case "Username":
                    return ValidateUsername();
                case "ColorString":
                    return ValidateColorString();
            }
            return null;
        }

        private string ValidateUsername()
        {
            if (String.IsNullOrWhiteSpace(Username))
                return "Username cannot be empty.";

            return null;
        }

        private string ValidateColorString()
        {
            var regex = new Regex(@"^#(?:[0-9a-fA-F]{3}){1,2}$");
            if (ColorString == null || !regex.IsMatch(ColorString)) 
                return "Invalid color.";

            return null;
        }

        #endregion
    }
}
