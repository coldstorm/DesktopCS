using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace DesktopCS.Helpers
{
    public static class CountryNameHelper
    {
        public static string GetCountryNameFromCC(string countryCode)
        {
            try
            {
                WebRequest request = WebRequest.Create(String.Format("http://api.worldbank.org/countries/{0}/", countryCode));
                request.Timeout = 2000;

                using (WebResponse res = request.GetResponse())
                {
                    using (Stream responseStream = res.GetResponseStream())
                    {
                        var xml = new XmlDocument();
                        if (responseStream != null)
                            xml.Load(responseStream);
                        XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
                        nsMgr.AddNamespace("wb", "http://www.worldbank.org");
                        XmlNode countryName = xml.SelectSingleNode("/wb:countries/wb:country/wb:name", nsMgr);
                        if (countryName != null)
                            return countryName.InnerText;
                    }
                }
            }

            catch
            {
                Debug.WriteLine(String.Format("Could not find country name of {0}", countryCode));
            }

            return "Unknown";
        }
    }
}
