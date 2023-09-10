namespace Phos.MusicManager.Library.Games;

/// <summary>
/// Game factory interface.
/// </summary>
public interface IGameFactory
{
    /// <summary>
    /// Gets an existing game instance or creates a new one.
    /// </summary>
    /// <param name="name">Name of game.</param>
    /// <returns>Game instance.</returns>
    Game GetGame(string name);
}
