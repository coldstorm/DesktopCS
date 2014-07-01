using System.Text.RegularExpressions;
using System.Windows.Media;
using DesktopCS.Models;

namespace DesktopCS.Helpers.Parsers
{
    public static class IdentHelper
    {
        public static UserMetadata Parse(string ident)
        {
            var regexp = new Regex(@"^([0-9a-f]{3}|[0-9a-f]{6})([0-9a-z]{2})$", RegexOptions.IgnoreCase);
            Match match = regexp.Match(ident);

            if (match.Success)
            {
                var color = ColorHelper.FromHexWithoutHash(match.Groups[1].Value);
                var flag = match.Groups[2].Value;
                var countryName = CountryNameHelper.GetCountryNameFromCC(flag);
                return new UserMetadata(color, flag, countryName);
            }

            return new UserMetadata(ColorHelper.ChatColor, null, null);
        }

        public static string Generate(Color color, string cc)
        {
            return Generate(ColorHelper.ToHexWithoutHash(color), cc);
        }

        public static string Generate(string hex, string cc)
        {
            return string.Format("{0}{1}", hex, cc);
        }
    }
}
