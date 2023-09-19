namespace Phos.MusicManager.Library.Projects;

using ProtoBuf;

[ProtoContract]
#pragma warning disable SA1600 // Elements should be documented
public class PresetAudioTrack
{
    [ProtoMember(1)]
    public string Name { get; set; } = string.Empty;

    [ProtoMember(2)]
    public string? Category { get; set; }

    [ProtoMember(3)]
    public string[] Tags { get; set; } = Array.Empty<string>();

    [ProtoMember(4)]
    public string? OutputPath { get; set; }

    [ProtoMember(6)]
    public string? Encoder { get; set; }
}
