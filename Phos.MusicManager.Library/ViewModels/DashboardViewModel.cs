namespace Phos.MusicManager.Library.ViewModels;

using System.ComponentModel;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.ViewModels.Services;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class DashboardViewModel : ViewModelBase
{
    private readonly ISavable<AppSettings> settings;
    private string selectedPage;

    public DashboardViewModel(DashboardService dashboard, ISavable<AppSettings> settings)
    {
        this.Navigation = dashboard.Navigation;
        this.settings = settings;

        // Workspace pages as top menu items.
        this.MenuItems = dashboard.Navigation.Pages
            .Where(x => x.GetType() == typeof(WorkspaceViewModel))
            .Select(x => x.Name)
            .ToArray();

        // Other pages as bottom menu items.
        this.FooterMenuItems = dashboard.Navigation.Pages
            .Where(x => !this.MenuItems.Contains(x.Name))
            .Select(x => x.Name)
            .ToArray();

        // Set initial page (last opened project or home page).
        this.selectedPage = settings.Value.CurrentProject ?? "Home";
        this.Navigation.NavigateTo(this.selectedPage);

        // Keep selected page in sync with navigation page.
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
                this.settings.Value.CurrentProject = this.Navigation.Current?.Name;
                this.settings.Save();
            }

            // Update selected.
            this.SelectedPage = this.Navigation.Current?.Name ?? "Home";
        }
    }
}
