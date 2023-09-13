namespace Phos.MusicManager.Library.ViewModels;

using System.ComponentModel;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Navigation;

/// <summary>
/// Dashboard view model.
/// </summary>
public partial class DashboardViewModel : ViewModelBase
{
    private readonly ISavable<AppSettings> settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DashboardViewModel"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="gameMenuItems"></param>
    /// <param name="appMenuItems"></param>
    public DashboardViewModel(ISavable<AppSettings> settings, IEnumerable<IPage> gameMenuItems, IEnumerable<IPage> appMenuItems)
    {
        this.settings = settings;

        // Menu items.
        this.GameMenuItems = gameMenuItems;
        this.FooterMenuItems = appMenuItems;

        // Navigation.
        var pages = this.GameMenuItems.Concat(this.FooterMenuItems);
        this.Navigation = new NavigationService(pages);
        this.Navigation.NavigateTo(settings.Value.CurrentGame);

        this.Navigation.PropertyChanged += this.Navigation_PropertyChanged;
    }

    /// <summary>
    /// Gets navigation.
    /// </summary>
    public NavigationService Navigation { get; }

    /// <summary>
    /// Gets game menu items.
    /// </summary>
    public IEnumerable<IPage> GameMenuItems { get; }

    /// <summary>
    /// Gets app footer menu items.
    /// </summary>
    public IEnumerable<IPage> FooterMenuItems { get; }

    private void Navigation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(this.Navigation.Current))
        {
            // Update selected game in app settings, if game item selected.
            if (this.GameMenuItems.Contains(this.Navigation.Current))
            {
                this.settings.Value.CurrentGame = this.Navigation.Current?.Name ?? Constants.P4G_PC_64;
                this.settings.Save();
            }
        }
    }
}
