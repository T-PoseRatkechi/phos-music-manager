namespace Phos.MusicManager.Library.ViewModels;

using System.Collections.Specialized;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.ViewModels.Projects;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class DashboardViewModel : ViewModelBase
{
    private readonly ISavable<AppSettings> appSettings;

    [ObservableProperty]
    private IPage[] menuItems;

    public DashboardViewModel(ProjectsNavigation navigation, ISavable<AppSettings> appSettings)
    {
        this.appSettings = appSettings;
        this.Navigation = navigation;

        // Generate menu items for pages.
        this.menuItems = this.Navigation.Pages.Where(x => x is ProjectViewModel).ToArray();
        this.FooterMenuItems = this.Navigation.Pages.Except(this.MenuItems).ToArray();

        // Set initial page.
        if (appSettings.Value.RestorePreviousProject && appSettings.Value.PreviousProject != null)
        {
            if (!this.Navigation.NavigateTo(appSettings.Value.PreviousProject))
            {
                this.Navigation.NavigateTo<HomeViewModel>();
            }
        }
        else
        {
            this.Navigation.NavigateTo<HomeViewModel>();
        }

        // Update app settings.
        this.Navigation.PropertyChanged += this.Navigation_PropertyChanged;

        // Update menu items on projects add/remove.
        this.Navigation.Pages.CollectionChanged += this.Pages_CollectionChanged;
    }

    public INavigationService Navigation { get; }

    public IPage[] FooterMenuItems { get; }

    private void Navigation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(this.Navigation.Current))
        {
            if (this.Navigation.Current == null)
            {
                this.Navigation.NavigateTo<HomeViewModel>();
                return;
            }

            if (this.MenuItems.Contains(this.Navigation.Current))
            {
                this.appSettings.Value.PreviousProject = this.Navigation.Current.Name;
                this.appSettings.Save();
            }
        }
    }

    private void Pages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.MenuItems = this.Navigation.Pages.Where(x => x is ProjectViewModel).ToArray();
    }
}
