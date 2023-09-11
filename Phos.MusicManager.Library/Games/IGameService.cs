namespace Phos.MusicManager.Library.Games;

/// <summary>
/// Game service interface.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Gets list of available games.
    /// </summary>
    IList<Game> Games { get; }
}
