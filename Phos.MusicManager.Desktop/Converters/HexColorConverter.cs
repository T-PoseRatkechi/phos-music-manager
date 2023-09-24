using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Themes.Fluent;
using FluentAvalonia.Core;
using FluentAvalonia.Styling;
using System;
using System.Globalization;
using System.Linq;

namespace Phos.MusicManager.Desktop.Converters;

public class HexColorConverter : IValueConverter
{
    private readonly IBrush defaultBrush;

    public HexColorConverter()
    {
        var theme = App.Current?.ActualThemeVariant?.ToString() ?? "Dark";
        if (theme == "Dark")
        {
            this.defaultBrush = Brush.Parse("White");
        }
        else
        {
            this.defaultBrush = Brush.Parse("Black");
        }
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            if (value is string color)
            {
                return Brush.Parse(color);
            }

            return this.defaultBrush;
        }
        catch (Exception)
        {
            return this.defaultBrush;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
