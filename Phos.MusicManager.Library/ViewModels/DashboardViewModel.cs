namespace Phos.MusicManager.Library.ViewModels;

using System.ComponentModel;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Navigation;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class DashboardViewModel : ViewModelBase
{
    private readonly ISavable<AppSettings> settings;
    private string selectedPage;

    public DashboardViewModel(ISavable<AppSettings> settings, NavigationService navigation, string[]? footerMenuItems = null)
    {
        this.settings = settings;

        this.Navigation = navigation;
        this.MenuItems = footerMenuItems == null ? navigation.AvailablePages : navigation.AvailablePages.Except(footerMenuItems).ToArray();
        this.FooterMenuItems = footerMenuItems ?? Array.Empty<string>();

        // Set initial page.
        this.Navigation.NavigateTo(settings.Value.CurrentGame);
        this.selectedPage = settings.Value.CurrentGame;

        this.Navigation.PropertyChanged += this.Navigation_PropertyChanged;
    }

    public NavigationService Navigation { get; }

    public string SelectedPage
    {
        get => this.selectedPage;
        set
        {
            this.SetProperty(ref this.selectedPage, value);
            this.Navigation.NavigateTo(value);
        }
    }

    public string[] MenuItems { get; }

    public string[] FooterMenuItems { get; }

    private void Navigation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(this.Navigation.Current))
        {
            // Update selected game in app settings, if game item selected.
            if (this.MenuItems.Contains(this.Navigation.Current?.Name))
            {
                this.settings.Value.CurrentGame = this.Navigation.Current?.Name ?? Constants.P4G_PC_64;
                this.settings.Save();
            }
        }
    }
}
