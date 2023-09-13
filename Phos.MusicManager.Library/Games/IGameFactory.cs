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
    /// Gets current games.
    /// </summary>
    /// <returns>List of games.</returns>
    Game[] GetGames();
}
