namespace Phos.MusicManager.Library;

using CommunityToolkit.Mvvm.ComponentModel;

#pragma warning disable SA1601 // Partial elements should be documented
public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    private string currentGame = Constants.P4G_PC_64;

    [ObservableProperty]
    private bool debugEnabled;
}
