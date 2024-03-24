using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace TouchlessWhiteboard.Converters;
public class FirstLetterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
        {
            return string.Empty;
        }

        string name = value as string;
        if (name == null)
        {
            // Value is not a string, handle this case as needed
            return string.Empty;
        }

        return string.IsNullOrEmpty(name) ? string.Empty : name.Substring(0, 1);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
