namespace Phos.MusicManager.Library.ViewModels;

using System.Collections.ObjectModel;
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
    private MenuItem? selectedItem;

    public DashboardViewModel(DashboardService dashboard, ISavable<AppSettings> settings)
    {
        this.Navigation = dashboard.Navigation;
        this.settings = settings;

        // Generate menu items from pages.
        foreach (var page in this.Navigation.Pages)
        {
            if (page is WorkspaceViewModel workspace)
            {
                this.MenuItems.Add(new(workspace.Name, workspace.Color));
            }
            else
            {
                this.FooterMenuItems.Add(new(page.Name));
            }
        }

        // Set initial page (last opened project or home page).
        this.SelectedItem = this.GetSelectedItem(this.settings.Value.CurrentProject);

        // Keep selected page in sync with navigation page.
        this.Navigation.PropertyChanged += this.Navigation_PropertyChanged;
    }

    public NavigationService Navigation { get; }

    public MenuItem? SelectedItem
    {
        get => this.selectedItem;
        set
        {
            this.SetProperty(ref this.selectedItem, value);
            this.Navigation.NavigateTo(this.selectedItem?.Name ?? "Home");
        }
    }

    public ObservableCollection<MenuItem> MenuItems { get; } = new();

    public List<MenuItem> FooterMenuItems { get; } = new();

    private void Navigation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(this.Navigation.Current))
        {
            // Update selected project in app settings, if project item selected.
            if (this.MenuItems.FirstOrDefault(x => x.Name == this.Navigation.Current?.Name) != null)
            {
                this.settings.Value.CurrentProject = this.Navigation.Current?.Name;
                this.settings.Save();
            }

            // Update selected.
            this.SelectedItem = this.GetSelectedItem(this.Navigation.Current?.Name);
        }
    }

    private MenuItem? GetSelectedItem(string? name)
    {
        var items = this.MenuItems.Concat(this.FooterMenuItems);
        return items.FirstOrDefault(x => x.Name == name);
    }

    public record MenuItem(string Name, string? Color = null);
}
