using DesktopCS.Models;
using DesktopCS.MVVM;
using DesktopCS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.ViewModels
{
    class SettingsViewModel : ObservableObject
    {
        public IRCSettings SettingsData { get; private set; }

        public SettingsViewModel()
        {
            this.SettingsData = SettingsManager.Value.GetIRCSettings();
        }
    }
}
