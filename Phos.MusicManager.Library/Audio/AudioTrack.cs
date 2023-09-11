﻿namespace Phos.MusicManager.Library.Audio;

#pragma warning disable SA1600 // Elements should be documented
public class AudioTrack
{
    public string Name { get; set; } = "Unknown";

    public string? Category { get; set; }

    public string[] Tags { get; set; } = Array.Empty<string>();

    public string? OutputPath { get; set; }

    public string? ReplacementFile { get; set; }

    public string? Processor { get; set; }
}
