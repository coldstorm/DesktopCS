using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace DesktopCS.Models
{
    public class LoginData : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _username;
        private string _password;
        private Color _color;

        public LoginData(string username, string password, Color color)
        {
            this._username = username;
            this._password = password;
            this._color = color;
        }

        public string Username
        {
            get { return this._username; }
            set 
            {
                this._username = value;
                this.OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return this._password; }
            set
            {
                this._password = value;
                this.OnPropertyChanged("Password");
            }
        }

        public Color Color
        {
            get { return this._color; }
            set
            {
                this._color = value;
                this.OnPropertyChanged("Color");
            }
        }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region IDataErrorInfo Members
        public string this[string columnName]
        {
            get
            {
                this.Error = this.GetValidationError(columnName);
                return this.Error;
            }
        }

        public string Error { get; private set; }
        #endregion

        #region Validation

        public bool IsValid
        {
            get { return this._validatedProperties.All(property => this.GetValidationError(property) == null); }
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
                    return this.ValidateUsername();
            }
            return null;
        }

        private string ValidateUsername()
        {
            var regex = new Regex(@"^[a-z_\-\[\]\\^{}|`][a-z0-9_\-\[\]\\^{}|`]*$", RegexOptions.IgnoreCase);
            if (this.Username == null || !regex.IsMatch(this.Username)) 
               return "Invalid Username.";

            return null;
        }

        #endregion
    }
}
