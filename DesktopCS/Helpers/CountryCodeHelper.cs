using System.Diagnostics;
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
                xml.Load("http://www.geoplugin.net/xml.gp");
                XmlNode countryCode = xml.SelectSingleNode("/geoPlugin/geoplugin_countryCode");
                if (countryCode != null)
                    return countryCode.InnerText;
            }
            catch
            {
                Debug.WriteLine("GeoIP lookup failed.");
            }

            return "QQ";
        }
    }
}
