namespace Phos.MusicManager.Library.Games;

using CommunityToolkit.Mvvm.ComponentModel;

#pragma warning disable SA1601 // Partial elements should be documented
public partial class GameSettings : ObservableObject
{
    [ObservableProperty]
    private string theme = "Dark";

    [ObservableProperty]
    private string? installPath;

    [ObservableProperty]
    private string? outputDir;
}
