using System.Windows.Media;

namespace DesktopCS.Models
{
    public class UserMetadata
    {
        public UserMetadata(Color color, string countryCode)
        {
            this.Color = color;
            this.CountryCode = countryCode;
        }

        public Color Color { get; private set; }
        public string CountryCode { get; private set; }
    }
}
