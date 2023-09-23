using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Phos.MusicManager.Desktop.Converters;

public class HexColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            if (value is string color)
            {
                return Brush.Parse(color);
            }
            else
            {
                return Brush.Parse("Turquoise");
            }
        }
        catch (Exception)
        {
            return Brush.Parse("Turquoise");
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
