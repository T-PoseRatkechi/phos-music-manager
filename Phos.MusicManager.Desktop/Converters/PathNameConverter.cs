using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.IO;

namespace Phos.MusicManager.Desktop.Converters;

public class PathNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string path)
        {
            try
            {
                return Path.GetFileName(path);
            }
            catch (Exception)
            {
                return path;
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
