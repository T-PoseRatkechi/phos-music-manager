namespace Phos.MusicManager.Library.ViewModels;

using Phos.MusicManager.Desktop.Library.ViewModels;

/// <summary>
/// Dashboard view model.
/// </summary>
public class DashboardViewModel : ViewModelBase
{
    /// <summary>
    /// Gets or sets name of current game.
    /// </summary>
    public string CurrentGame { get; set; } = "Persona 4 Golden";
}
