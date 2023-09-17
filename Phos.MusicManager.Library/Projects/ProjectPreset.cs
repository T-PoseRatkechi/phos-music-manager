﻿namespace Phos.MusicManager.Library.Projects;

using Phos.MusicManager.Library.Audio.Models;
using ProtoBuf;

#pragma warning disable SA1600 // Elements should be documented
[ProtoContract]
public class ProjectPreset
{
    [ProtoMember(1)]
    public string Name { get; set; } = string.Empty;

    [ProtoMember(2)]
    public string? Color { get; set; }

    [ProtoMember(3)]
    public AudioTrack[] DefaultTracks { get; set; } = Array.Empty<AudioTrack>();

    [ProtoMember(4)]
    public string? PostBuild { get; set; } = null;

    [ProtoMember(5)]
    public byte[]? Icon { get; set; }
}
