using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesktopCS.Models
{
    class ChatData : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _inputText;

        public string InputText
        {
            get { return this._inputText; }
            set
            {
                this._inputText = value;
                this.OnPropertyChanged("InputText");
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
                "InputText"
            };

        private string GetValidationError(string columnName)
        {
            switch (columnName)
            {
                case "InputText":
                    return this.ValidateInputText();
            }
            return null;
        }

        private string ValidateInputText()
        {
            if (String.IsNullOrWhiteSpace(this.InputText))
                return "Text input is empty.";

            return null;
        }

        #endregion
    }
}
