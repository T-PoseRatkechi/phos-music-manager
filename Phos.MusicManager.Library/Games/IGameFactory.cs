namespace Phos.MusicManager.Library.Games;

/// <summary>
/// Game factory interface.
/// </summary>
public interface IGameFactory
{
    /// <summary>
    /// Create game instance.
    /// </summary>
    /// <param name="name">Name of game.</param>
    /// <returns>Game instance.</returns>
    Game CreateGame(string name);

    /// <summary>
    /// Get all available games.
    /// </summary>
    /// <returns>All existing games.</returns>
    IEnumerable<Game> GetGames();
}
