namespace Phos.MusicManager.Library.Audio;

using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common.Serializers;

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
    {
        var gameMusicFile = Path.Join(DefaultMusicDir, $"{name}.json");
        return File.Exists(gameMusicFile) ? gameMusicFile : null;
    }

    /// <summary>
    /// Gets default music audio tracks for game.
    /// </summary>
    /// <param name="name">Game name.</param>
    /// <returns>Default audio tracks.</returns>
    public static AudioTrack[] GetDefaultMusic(string name)
    {
        var musicFile = GetDefaultMusicFile(name);
        if (musicFile == null)
        {
            return Array.Empty<AudioTrack>();
        }

        try
        {
            return JsonFileSerializer.Deserialize<AudioTrack[]>(musicFile) ?? Array.Empty<AudioTrack>();
        }
        catch (Exception)
        {
            return Array.Empty<AudioTrack>();
        }
    }
}
