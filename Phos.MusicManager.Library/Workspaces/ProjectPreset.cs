namespace Phos.MusicManager.Library.Workspaces;

using Phos.MusicManager.Library.Audio.Models;

/// <summary>
/// Project preset.
/// </summary>
public class ProjectPreset
{
    /// <summary>
    /// Gets or sets preset name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets preset target game.
    /// </summary>
    public string Game { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets preset icon color.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets preset default tracks.
    /// </summary>
    public AudioTrack[] DefaultTracks { get; set; } = Array.Empty<AudioTrack>();

    /// <summary>
    /// Gets or sets preset post build.
    /// </summary>
    public string? PostBuild { get; set; } = null;
}
