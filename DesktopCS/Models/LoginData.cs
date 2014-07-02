using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using DesktopCS.MVVM;

namespace DesktopCS.Models
{
    public class LoginData : ObservableObject, IDataErrorInfo
    {
        private string _username;

        public string Username
        {
            get { return this._username; }
            set 
            {
                this._username = value;
                this.OnPropertyChanged("Username");
            }
        }

        private string _password;

        public string Password
        {
            get { return this._password; }
            set
            {
                this._password = value;
                this.OnPropertyChanged("Password");
            }
        }

        private string _channels;

        public string Channels
        {
            get { return this._channels; }
            set
            {
                this._channels = value;
                this.OnPropertyChanged("Channels");
            }
        }

        private Color _color;

        public Color Color
        {
            get { return this._color; }
            set
            {
                this._color = value;
                this.OnPropertyChanged("Color");
            }
        }

        public LoginData(string username, string password, string channels, Color color)
        {
            this._username = username;
            this._password = password;
            this._color = color;
            this._channels = channels;
        }

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
                case "Channels":
                    return this.ValidateChannels();
            }
            return null;
        }

        private string ValidateUsername()
        {
            var regex = new Regex(@"^[a-z_\-\[\]\\^{}|`][a-z0-9_\-\[\]\\^{}|`]{0,30}$", RegexOptions.IgnoreCase);
            if (string.IsNullOrEmpty(this.Username) || !regex.IsMatch(this.Username)) 
               return "Invalid Username.";

            return null;
        }

        private string ValidateChannels()
        {
            var regex = new Regex(@"^([#&][a-zA-Z0-9]{1,25})$", RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(this.Channels))
            {
                string[] channelNames = this.Channels.Split(',');

                //Check to make sure each channel is valid
                for (var i = 0; i < channelNames.Length; i++)
                {
                    if (!regex.IsMatch(channelNames[i]))
                        return "Invalid Channel Name. Try #Coldstorm.";
                }
            }
            else
                return "No Channel Selected.";

            return null;
        }
        #endregion
    }
}
