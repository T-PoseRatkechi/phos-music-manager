namespace Phos.MusicManager.Library.Audio;

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
}
