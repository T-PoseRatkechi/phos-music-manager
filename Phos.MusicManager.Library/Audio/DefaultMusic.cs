namespace Phos.MusicManager.Library.Audio;

using Phos.MusicManager.Library.Metadata;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.Serializers;

/// <summary>
/// Default music provider.
/// </summary>
public static class DefaultMusic
{
    private static readonly string DefaultMusicDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Music");

    /// <summary>
    /// Gets the default music file for game.
    /// </summary>
    /// <param name="name">Game name.</param>
    /// <returns>Path to default music file.</returns>
    public static string? GetDefaultMusicFile(string name)
        => name switch
        {
            Constants.P4G_PC_64 => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "P4G_PC", "music.yaml"),
            Constants.P3P_PC => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "P3P_PC", "music.yaml"),
            Constants.P5R_PC => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "P5R_PC", "music.yaml"),
            Constants.P3R_PC => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "P3R_PC", "music.yaml"),
            _ => null,
        };

    /// <summary>
    /// Gets default music audio tracks for game.
    /// </summary>
    /// <param name="name">Game name.</param>
    /// <returns>Default audio tracks.</returns>
    public static PresetAudioTrack[] GetDefaultMusic(string name)
    {
        var musicFile = GetDefaultMusicFile(name);
        if (musicFile == null)
        {
            return Array.Empty<PresetAudioTrack>();
        }

        try
        {
            var gameMusic = YamlSerializer.Deserialize<GameMusic>(musicFile)!;
            return gameMusic.Songs.Select(x => new PresetAudioTrack()
            {
                Name = x.Name,
                Category = x.Category,
                Encoder = x.Encoder,
                OutputPath = x.ReplacementPath,
                Tags = x.Tags,
            }).ToArray();
        }
        catch (Exception)
        {
            return Array.Empty<PresetAudioTrack>();
        }
    }
}
