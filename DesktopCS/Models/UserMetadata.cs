using System.Windows.Media;

namespace DesktopCS.Models
{
    public class UserMetadata
    {
        public UserMetadata(SolidColorBrush color, string countryCode)
        {
            Color = color;
            CountryCode = countryCode;
        }

        public SolidColorBrush Color { get; private set; }
        public string CountryCode { get; private set; }
    }
}
