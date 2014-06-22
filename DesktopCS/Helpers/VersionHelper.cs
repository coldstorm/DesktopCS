using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.Helpers
{
    public static class VersionHelper
    {
        public static string GetPublishedVersion()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.
                    CurrentVersion.ToString();
            }
            return "Not network deployed";
        }
    }
}
