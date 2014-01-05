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
        private bool _pingSound;

        public bool PingSound
        {
            get { return this._pingSound; }
            set
            {
                this._pingSound = value;
                this.OnPropertyChanged("PingSound");
            }
        }

        public IRCSettings(bool pingSound)
        {
            this.PingSound = pingSound;
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
            //switch (columnName)
            //{
            //}
            return null;
        }

        #endregion
    }
}
