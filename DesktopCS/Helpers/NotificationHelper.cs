using DesktopCS.Helpers.Extensions;
using DesktopCS.Helpers.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesktopCS.Helpers
{
    public static class NotificationHelper
    {
        public static void Notify(ParseArgs args)
        {
            // Play the ping sound if it is enabled
            if (args.SoundNotification)
                SoundHelper.PlaySound(SoundHelper.PingUri);

            // Flash the window indefinitely if desktop notifications are enabled
            // TODO: Add setting for flashing window X number of times
            if (args.DesktopNotification)
                WindowExtensions.FlashWindow(Application.Current.MainWindow);
        }
    }
}
