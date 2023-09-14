using Avalonia.Data.Converters;
using Phos.MusicManager.Library.Workspaces;
using System;
using System.Globalization;
using System.IO;

namespace Phos.MusicManager.Desktop.Converters;

public class WorkspaceIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Workspace workspace)
        {
            var iconFile = Path.Join(workspace.WorkspaceFolder, "icon.png");
            if (File.Exists(iconFile))
            {
                var icon = new Avalonia.Media.Imaging.Bitmap(iconFile);
                return icon;
            }

            var resourceIconFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "resources", "icons", $"{workspace.Settings.Value.Game}.png");
            if (File.Exists(resourceIconFile))
            {
                var icon = new Avalonia.Media.Imaging.Bitmap(resourceIconFile);
                return icon;
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
