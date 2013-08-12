using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace DesktopCS.Models
{
    class LoginData : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _username;
        private string _password;
        private SolidColorBrush _colorBrush = Brushes.White;

        public LoginData(string username, string password, SolidColorBrush colorBrush)
        {
            _username = username;
            _password = password;
            _colorBrush = colorBrush;
        }

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

        public SolidColorBrush ColorBrush
        {
            get { return _colorBrush; }
            set
            {
                _colorBrush = value;
                OnPropertyChanged("ColorBrush");
            }
        }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region IDataErrorInfo Members
        public string this[string columnName]
        {
            get
            {
                Error = GetValidationError(columnName);
                return Error;
            }
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
                "Username"
            };

        private string GetValidationError(string columnName)
        {
            switch (columnName)
            {
                case "Username":
                    return ValidateUsername();
            }
            return null;
        }

        private string ValidateUsername()
        {
            var regex = new Regex(@"^[a-z_\-\[\]\\^{}|`][a-z0-9_\-\[\]\\^{}|`]*$", RegexOptions.IgnoreCase);
            if (Username == null || !regex.IsMatch(Username)) 
               return "Invalid Username.";

            return null;
        }

        #endregion
    }
}
