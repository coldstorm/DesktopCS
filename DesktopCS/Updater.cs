using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;
using System.Windows.Forms;

namespace DesktopCS
{
    public static class Updater
    {
        public async static void CheckForUpdates()
        {
            WebClient client = new WebClient();
            string latestVersionString = await client.DownloadStringTaskAsync(new Uri("https://dl.dropboxusercontent.com/s/ul840r0b2b5o6r4/version.txt"));
            Version latestVersion = new Version(latestVersionString);
            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

            if (latestVersion.CompareTo(currentVersion) > 0)
            {
                if (MessageBox.Show("A new update for DesktopCS is available. Would you like to update now?", "DesktopCS Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DownloadUpdate();
                }
            }
        }

        public async static void DownloadUpdate()
        {
            WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri("https://dl.dropboxusercontent.com/s/9wcewrkxcrgxujs/DesktopCS.exe"), Application.ExecutablePath);
            Application.Exit();
        }
    }
}
