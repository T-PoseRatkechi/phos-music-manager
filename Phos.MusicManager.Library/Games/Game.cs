namespace Phos.MusicManager.Library.Games;

/// <summary>
/// Game instance.
/// </summary>
public class Game
{
    /// <summary>
    /// Gets or sets the game name.
    /// </summary>
    public string Name { get; set; } = "Unknown Game";

    /// <summary>
    /// Gets or sets the output directory.
    /// </summary>
    public string? OutputDir { get; set; }
}
