namespace Phos.MusicManager.Library.Games;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Phos.MusicManager.Library.Common;

/// <summary>
/// Game service.
/// </summary>
public partial class GameService : ObservableObject, IGameService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameService"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="gameFactory"></param>
    public GameService(ISavable<AppSettings> settings, IGameFactory gameFactory)
    {
        this.Games = new ObservableCollection<Game>(gameFactory.GetGames());
    }

    /// <inheritdoc/>
    public IList<Game> Games { get; }
}
