namespace Phos.MusicManager.Library.ViewModels;

using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Games;
using Phos.MusicManager.Library.Navigation;

/// <summary>
/// Settings view model.
/// </summary>
public class SettingsViewModel : ViewModelBase, IPage
{
    private readonly ISavable<AppSettings> settings;
    private readonly IGameService gameService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="gameService"></param>
    public SettingsViewModel(ISavable<AppSettings> settings, IGameService gameService)
    {
        this.settings = settings;
        this.gameService = gameService;
    }

    /// <inheritdoc/>
    public string Name { get; } = "Settings";

    /// <summary>
    /// Gets or sets a value indicating whether debug mode is enabled.
    /// </summary>
    public bool DebugEnabled
    {
        get => this.settings.Value.DebugEnabled;
        set
        {
            this.settings.Value.DebugEnabled = value;
            this.settings.Save();
        }
    }
}
