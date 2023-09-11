namespace Phos.MusicManager.Library.ViewModels;

using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Games;
using Phos.MusicManager.Library.Navigation;

/// <summary>
/// Game hub view model.
/// </summary>
public class GameHubViewModel : ViewModelBase, IPage
{
    private readonly Game game;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameHubViewModel"/> class.
    /// </summary>
    /// <param name="game"></param>
    public GameHubViewModel(Game game)
    {
        this.game = game;
        this.Name = game.Name;
    }

    /// <inheritdoc/>
    public string Name { get; }
}
