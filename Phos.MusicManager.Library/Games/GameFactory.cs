namespace Phos.MusicManager.Library.Games;

using Phos.MusicManager.Library.Common;

/// <summary>
/// Game factory.
/// </summary>
public class GameFactory : IGameFactory
{
    private readonly string gamesDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "games");

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
}
