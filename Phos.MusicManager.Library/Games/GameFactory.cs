namespace Phos.MusicManager.Library.Games;

using System.Collections.Generic;
using Phos.MusicManager.Library.Common;

/// <summary>
/// Game factory.
/// </summary>
public class GameFactory : IGameFactory
{
    private readonly string gamesDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "games");
    private readonly string[] knownGames = new string[]
    {
        Constants.P4G_PC_64,
        Constants.P3P_PC,
        Constants.P5R_PC,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="GameFactory"/> class.
    /// </summary>
    public GameFactory()
    {
        Directory.CreateDirectory(this.gamesDir);
    }

    /// <inheritdoc/>
    public Game GetGame(string name)
    {
        var gameFolder = Path.Join(this.gamesDir, name);
        Directory.CreateDirectory(gameFolder);

        var settingsFile = Path.Join(gameFolder, "game-settings.json");
        var settings = new SavableFile<GameSettings>(settingsFile);
        var game = new Game(name, settings);
        return game;
    }

    /// <inheritdoc/>
    public IEnumerable<Game> GetGames()
    {
        var games = new List<Game>();

        // Load known games.
        foreach (var knownGame in this.knownGames)
        {
            var game = this.GetGame(knownGame);
            games.Add(game);
        }

        // Load custom games.
        foreach (var dir in Directory.EnumerateDirectories(this.gamesDir))
        {
            var gameName = Path.GetFileName(dir);
            if (gameName == null || this.knownGames.Contains(gameName))
            {
                continue;
            }

            var game = this.GetGame(gameName);
            games.Add(game);
        }

        return games;
    }
}
