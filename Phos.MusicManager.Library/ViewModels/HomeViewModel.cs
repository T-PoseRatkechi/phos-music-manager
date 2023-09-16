namespace Phos.MusicManager.Library.ViewModels;

using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels.Projects;
using Phos.MusicManager.Library.ViewModels.Projects.Factories;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class HomeViewModel : ViewModelBase, IPage
{
    private readonly ProjectsNavigation navigation;
    private readonly CreateProjectFactory createProjectFactory;
    private readonly IDialogService dialog;

    [ObservableProperty]
    private ProjectViewModel[] projectPages;

    public HomeViewModel(
        ProjectsNavigation navigation,
        CreateProjectFactory createProjectFactory,
        IDialogService dialog)
    {
        this.navigation = navigation;
        this.createProjectFactory = createProjectFactory;
        this.dialog = dialog;

        this.projectPages = this.navigation.Pages.Where(x => x is ProjectViewModel).Cast<ProjectViewModel>().ToArray();
        navigation.Pages.CollectionChanged += this.Pages_CollectionChanged;
    }

    public string Name { get; } = "Home";

    [RelayCommand]
    private async Task CreateProject()
    {
        var createProject = this.createProjectFactory.Create();
        await this.dialog.OpenDialog<ProjectSettings>(createProject);
    }

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
