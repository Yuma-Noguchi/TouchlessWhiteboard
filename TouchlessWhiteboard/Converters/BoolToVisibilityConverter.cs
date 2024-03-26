using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchlessWhiteboard.Converters;
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool isVisible = (bool)value;
        if (parameter is string && parameter.ToString() == "True")
            isVisible = !isVisible;
        return isVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        Visibility visibility = (Visibility)value;
        bool isVisible = visibility == Visibility.Visible;
        if (parameter is string && parameter.ToString() == "True")
            isVisible = !isVisible;
        return isVisible;
    }
}
