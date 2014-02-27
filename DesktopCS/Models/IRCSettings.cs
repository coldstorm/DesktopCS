using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Windows.Media;
using DesktopCS.MVVM;

namespace DesktopCS.Models
{
    public class IRCSettings : ObservableObject, IDataErrorInfo
    {
        private bool _soundNotifications;

        public bool SoundNotifications
        {
            get { return this._soundNotifications; }
            set
            {
                this._soundNotifications = value;
                this.OnPropertyChanged("SoundNotifications");
            }
        }

        private bool _desktopNotifications;

        public bool DesktopNotifications
        {
            get { return this._desktopNotifications; }
            set
            {
                this._desktopNotifications = value;
                this.OnPropertyChanged("DesktopNotifications");
            }
        }

        public IRCSettings(bool soundNotifications, bool desktopNotifications)
        {
            this.SoundNotifications = soundNotifications;
            this.DesktopNotifications = desktopNotifications;
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
            };

        private string GetValidationError(string columnName)
        {
            return null;
        }

        #endregion
    }
}
