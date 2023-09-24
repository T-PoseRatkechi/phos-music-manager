using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Phos.MusicManager.Desktop.Converters;

public class ObjectEqualsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value?.Equals(parameter) ?? false)
        {
            return true;
        }

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
