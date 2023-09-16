using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Phos.MusicManager.Library.Projects;
using System;
using System.Globalization;
using System.IO;

namespace Phos.MusicManager.Desktop.Converters;

public class ProjectIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string filePath && File.Exists(filePath))
        {
            return new Bitmap(filePath);
        } 
        else if (value is Project project)
        {
            var projectIconFile = Path.Join(project.ProjectFolder, "icon.png");
            if (File.Exists(projectIconFile))
            {
                return new Bitmap(projectIconFile);
            }

            var resourceIconFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "resources", "icons", $"{project.Settings.Value.Preset}.png");
            if (File.Exists(resourceIconFile))
            {
                return new Bitmap(resourceIconFile);
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
