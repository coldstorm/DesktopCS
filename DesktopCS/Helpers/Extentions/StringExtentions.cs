using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopCS.Helpers.Extentions
{
    public static class StringExtentions
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
    }
}
