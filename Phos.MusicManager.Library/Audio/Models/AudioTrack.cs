namespace Phos.MusicManager.Library.Audio.Models;

using CommunityToolkit.Mvvm.ComponentModel;

#pragma warning disable SA1601 // Partial elements should be documented
public partial class AudioTrack : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? category;

    [ObservableProperty]
    private string[] tags = Array.Empty<string>();

    [ObservableProperty]
    private string? outputPath;

    [ObservableProperty]
    private string? replacementFile;

    [ObservableProperty]
    private string encoder = "HCA";

    [ObservableProperty]
    private Loop loop = new();
}
