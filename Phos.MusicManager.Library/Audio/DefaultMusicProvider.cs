namespace Phos.MusicManager.Library.Audio;

using Phos.MusicManager.Library.Common.Serializers;
using Phos.MusicManager.Library.Games;

/// <summary>
/// Default music provider.
/// </summary>
public class DefaultMusicProvider
{
    private readonly string defaultMusicDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Music");
    private readonly Dictionary<string, string> musicIds = new()
    {
        { Constants.P4G_PC_64, nameof(Constants.P4G_PC_64) },
        { Constants.P5R_PC, nameof(Constants.P5R_PC) },
        { Constants.P3P_PC, nameof(Constants.P3P_PC) },
    };

    /// <summary>
    /// Gets default music for game.
    /// </summary>
    /// <param name="game">Game to get default music for.</param>
    /// <returns>Default music for game, if found.</returns>
    public AudioTrack[]? GetDefaultMusic(Game game)
    {
        if (this.musicIds.TryGetValue(game.Name, out var id))
        {
            var gameMusicFile = Path.Join(this.defaultMusicDir, $"{id}.json");
            if (!File.Exists(gameMusicFile))
            {
                return null;
            }

            var gameMusic = JsonFileSerializer.Deserialize<AudioTrack[]>(gameMusicFile);
            return gameMusic;
        }

        return null;
    }
}
