using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DesktopCS.Helpers.Extensions;
using NetIRC;
using NetIRCHelper = DesktopCS.Helpers.NetIRCHelper;

namespace DesktopCS.Converters
{
    class RankConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rank = (UserRank)value;
            UserRank highest = EnumExtensions.GetFlags(rank).Max();
            if (NetIRCHelper.RankChars.ContainsKey(highest))
                return NetIRCHelper.RankChars[highest];

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
