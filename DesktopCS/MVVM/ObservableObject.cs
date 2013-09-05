using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DesktopCS.MVVM
{
    public class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        protected void VerifyPropertyName(String propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new Exception(String.Format("Invalid property name. Type: {0}, Name: {1}", GetType(), propertyName));
        }

        #endregion
    }
}
