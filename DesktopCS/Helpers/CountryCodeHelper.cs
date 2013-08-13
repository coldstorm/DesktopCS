using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace DesktopCS.Helpers
{
    static class CountryCodeHelper
    {
        public static string GetCC()
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load("http://freegeoip.net/xml");
                XmlNode countryCode = xml.SelectSingleNode("/Response/CountryCode");
                return countryCode.InnerText;
            }
            catch
            {
                Debug.WriteLine("GeoIP lookup failed.");
                return "QQ";
            }
        }
    }
}
