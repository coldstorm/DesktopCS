using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services;


namespace DesktopCS.ViewModels
{
    class SettingsViewModel : ObservableObject
    {
        public IRCSettings SettingsData { get; private set; }
        public ICommand SaveSettingsCommand { get; private set; }

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get { return this._dialogResult; }
            set
            {
                this._dialogResult = value;
                this.OnPropertyChanged("DialogResult");
            }
        }

        public SettingsViewModel()
        {
            this.SettingsData = SettingsManager.Value.GetIRCSettings();

            this.SaveSettingsCommand = new RelayCommand(param => this.SaveSettings(), param => this.CanSaveSettings);
        }

        public bool CanSaveSettings
        {
            get { return this.SettingsData.IsValid; }
        }

        public void SaveSettings()
        {
            SettingsManager.Value.SetIRCSettings(this.SettingsData);
            SettingsManager.Value.Save();
            this.DialogResult = true;
        }
    }
}
