using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;

namespace DesktopCS.Helpers
{
    public static class CountryCodeHelper
    {
        public static string GetCC()
        {
            try
            {
                WebRequest request = WebRequest.Create("http://www.geoplugin.net/xml.gp");
                request.Timeout = 2000;

                using (WebResponse res = request.GetResponse())
                {
                    using (Stream responseStream = res.GetResponseStream())
                    {
                        var xml = new XmlDocument();
                        if (responseStream != null)
                            xml.Load(responseStream);
                        XmlNode countryCode = xml.SelectSingleNode("/geoPlugin/geoplugin_countryCode");
                        if (countryCode != null)
                            return countryCode.InnerText;
                    }
                }
            }

            catch
            {
                Debug.WriteLine("GeoIP lookup failed.");
            }

            return "QQ";
        }
    }
}
