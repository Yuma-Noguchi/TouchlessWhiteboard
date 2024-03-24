using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace TouchlessWhiteboard.Converters;
public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool && (bool)value)
        {
            return new SolidColorBrush(Colors.Red); // Color when selected
        }
        else
        {
            return new SolidColorBrush(Colors.Green); // Default color
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
