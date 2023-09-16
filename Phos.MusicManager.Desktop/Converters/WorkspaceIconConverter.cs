using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Phos.MusicManager.Library.Workspaces;
using System;
using System.Globalization;
using System.IO;

namespace Phos.MusicManager.Desktop.Converters;

public class WorkspaceIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string filePath && File.Exists(filePath))
        {
            return new Bitmap(filePath);
        } 
        else if (value is Workspace workspace)
        {
            var workspaceIconFile = Path.Join(workspace.WorkspaceFolder, "icon.png");
            if (File.Exists(workspaceIconFile))
            {
                return new Bitmap(workspaceIconFile);
            }

            var resourceIconFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "resources", "icons", $"{workspace.Settings.Value.Preset}.png");
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
