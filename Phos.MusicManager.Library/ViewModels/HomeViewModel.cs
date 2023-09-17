namespace Phos.MusicManager.Library.ViewModels;

using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Commands;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.ViewModels.Projects;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class HomeViewModel : ViewModelBase, IPage
{
    private readonly ProjectsNavigation navigation;

    [ObservableProperty]
    private ProjectViewModel[] projectPages;

    public HomeViewModel(ProjectsNavigation navigation, ProjectCommands projectCommands)
    {
        this.navigation = navigation;

        this.projectPages = this.navigation.Pages.Where(x => x is ProjectViewModel).Cast<ProjectViewModel>().ToArray();
        navigation.Pages.CollectionChanged += this.Pages_CollectionChanged;

        this.NewProjectCommand = projectCommands.Create_NewProjectCommand();
    }

    public string Name { get; } = "Home";

    public IRelayCommand NewProjectCommand { get; }

    [RelayCommand]
    private void OpenProject(string projectName)
    {
        this.navigation.NavigateTo(projectName);
    }

    private void Pages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.ProjectPages = this.navigation.Pages.Where(x => x is ProjectViewModel).Cast<ProjectViewModel>().ToArray();
    }
}
