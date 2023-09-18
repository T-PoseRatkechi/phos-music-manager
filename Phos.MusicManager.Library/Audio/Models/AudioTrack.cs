namespace Phos.MusicManager.Library.Audio.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using ProtoBuf;

#pragma warning disable SA1601 // Partial elements should be documented
[ProtoContract]
public partial class AudioTrack : ObservableObject
{
    [ProtoMember(1)]
    [ObservableProperty]
    private string name = string.Empty;

    [ProtoMember(2)]
    [ObservableProperty]
    private string? category;

    [ProtoMember(3)]
    [ObservableProperty]
    private string[] tags = Array.Empty<string>();

    [ProtoMember(4)]
    [ObservableProperty]
    private string? outputPath;

    [ProtoMember(5)]
    [ObservableProperty]
    private string? replacementFile;

    [ProtoMember(6)]
    [ObservableProperty]
    private string? encoder;

    [ProtoMember(7)]
    [ObservableProperty]
    private Loop loop = new();
}
