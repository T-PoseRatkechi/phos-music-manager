using Avalonia.Data.Converters;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.ViewModels;
using System;
using System.Globalization;

namespace Phos.MusicManager.Desktop.Converters;

public class ProjectNavigationItemConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ProjectViewModel projectPage)
        {
            return projectPage.Project.Settings.Value.Name;
        }
        else if (value is IPage page)
        {
            return page.Name;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
