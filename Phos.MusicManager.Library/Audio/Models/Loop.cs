namespace Phos.MusicManager.Library.Audio.Models;

using CommunityToolkit.Mvvm.ComponentModel;

#pragma warning disable SA1601 // Partial elements should be documented
public partial class Loop : ObservableObject
{
    [ObservableProperty]
    private bool enabled;

    [ObservableProperty]
    private int startSample;

    [ObservableProperty]
    private int endSample;
}
