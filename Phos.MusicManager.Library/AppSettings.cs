namespace Phos.MusicManager.Library;

using CommunityToolkit.Mvvm.ComponentModel;

#pragma warning disable SA1601 // Partial elements should be documented
public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    private string? currentProject;

    [ObservableProperty]
    private List<string> projectFiles = new();

    [ObservableProperty]
    private bool debugEnabled;
}
