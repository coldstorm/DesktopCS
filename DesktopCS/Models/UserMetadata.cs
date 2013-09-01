using System.Windows.Media;

namespace DesktopCS.Models
{
    public class UserMetadata
    {
        public UserMetadata(SolidColorBrush color, string countryCode)
        {
            this.Color = color;
            this.CountryCode = countryCode;
        }

        public SolidColorBrush Color { get; private set; }
        public string CountryCode { get; private set; }
    }
}
