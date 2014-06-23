using System.Windows.Media;

namespace DesktopCS.Models
{
    public class UserMetadata
    {
        public Color Color { get; private set; }
        public string CountryCode { get; private set; }
        public string CountryName { get; private set; }

        public UserMetadata(Color color, string countryCode, string countryName)
        {
            this.Color = color;
            this.CountryCode = countryCode;
            this.CountryName = countryName;
        }
    }
}
