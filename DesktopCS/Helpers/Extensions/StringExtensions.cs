using System.Collections.Generic;

namespace DesktopCS.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitByLength(this string str, int maxLength)
        {
            int index = 0;
            while (index + maxLength <= str.Length)
            {
                yield return str.Substring(index, maxLength);
                index += maxLength;
            }

            yield return str.Substring(index);
        }

        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            int place = source.LastIndexOf(find, System.StringComparison.Ordinal);
            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
    }
}
