namespace Phos.MusicManager.Library.Commands;

using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

#pragma warning disable SA1600 // Elements should be documented
public static class OpenPathCommand
{
    public static IRelayCommand Create(string path) =>
        new RelayCommand(() => OpenPath(path));

    private static void OpenPath(string path)
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        ProcessStartInfo info = new()
        {
            UseShellExecute = true,
            FileName = path,
        };

        Process.Start(info);
    }
}
