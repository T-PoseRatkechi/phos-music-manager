using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Phos.MusicManager.Desktop.Converters;

public class NullBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is null;
    }
}
