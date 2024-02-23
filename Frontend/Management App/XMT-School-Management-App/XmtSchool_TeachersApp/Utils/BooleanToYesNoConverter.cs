using System;
using System.Globalization;
using System.Windows.Data;

namespace XmtSchool_TeachersApp.Utils
{
    public class BooleanToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null && (bool)value) ? "Yes" : "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Equals(value as string, "Yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}
