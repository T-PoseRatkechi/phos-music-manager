namespace Phos.MusicManager.Library.Audio.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using ProtoBuf;

#pragma warning disable SA1601 // Partial elements should be documented
[ProtoContract]
public partial class Loop : ObservableObject
{
    [ProtoMember(1)]
    [ObservableProperty]
    private bool enabled;

    [ProtoMember(2)]
    [ObservableProperty]
    private int startSample;

    [ProtoMember(3)]
    [ObservableProperty]
    private int endSample;
}
