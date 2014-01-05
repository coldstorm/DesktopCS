using System;
using System.IO;
using System.Media;
using System.Windows;

namespace DesktopCS.Helpers
{
    public static class SoundHelper
    {
        public const string PingUri = @"pack://application:,,,/Resources/cs_ping.wav";

        public static void PlaySound(string uriPath)
        {
            var uri = new Uri(uriPath);
            var streamResourceInfo = Application.GetResourceStream(uri);
            if (streamResourceInfo != null)
            {
                var player = new SoundPlayer(streamResourceInfo.Stream);
                player.Play();
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
