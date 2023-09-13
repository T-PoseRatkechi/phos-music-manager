namespace Phos.MusicManager.Library.Games;

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common.Serializers;

/// <summary>
/// Game factory.
/// </summary>
public class GameFactory : IGameFactory
{
    private readonly ILogger? log;
    private readonly string gamesDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "games");
    private readonly string[] supportedGames = new string[]
    {
        Constants.P4G_PC_64,
        Constants.P3P_PC,
        Constants.P5R_PC,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="GameFactory"/> class.
    /// </summary>
    /// <param name="log"></param>
    public GameFactory(ILogger? log = null)
    {
        this.log = log;
        Directory.CreateDirectory(this.gamesDir);
    }

    /// <inheritdoc/>
    public Game CreateGame(string name)
    {
        var gameFolder = Path.Join(this.gamesDir, name);
        var game = new Game(name, gameFolder);
        this.AddCustomTracks(game);

        return game;
    }

    /// <inheritdoc/>
    public Game[] GetGames()
    {
        var games = new List<Game>();

        // Add known games.
        foreach (var supportedGame in this.supportedGames)
        {
            try
            {
                var game = this.CreateGame(supportedGame);
                games.Add(game);
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to create game {game}.", supportedGame);
            }
        }

        // Add custom games.
        foreach (var dir in Directory.EnumerateDirectories(this.gamesDir))
        {
            var gameName = Path.GetFileName(dir);
            if (gameName == null || this.supportedGames.Contains(gameName))
            {
                continue;
            }

            try
            {
                var game = this.CreateGame(gameName);
                games.Add(game);
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to create game {game}.", gameName);
            }
        }

        return games.ToArray();
    }

    private void AddCustomTracks(Game game)
    {
        // Add new custom tracks.
        var customTracksDir = Directory.CreateDirectory(Path.Join(game.AudioFolder, "custom"));

        foreach (var customFile in Directory.EnumerateFiles(customTracksDir.FullName, "*.json", SearchOption.AllDirectories))
        {
            try
            {
                var customTracks = JsonFileSerializer.Deserialize<AudioTrack[]>(customFile)!;

                foreach (var customTrack in customTracks)
                {
                    if (game.Audio.Tracks.FirstOrDefault(x => x.OutputPath == customTrack.OutputPath) == null)
                    {
                        game.Audio.Tracks.Add(customTrack);
                        this.log?.LogInformation("Custom track added: {name}", customTrack.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to add custom tracks from file.\nFile: {file}", customFile);
            }
        }
    }
}
