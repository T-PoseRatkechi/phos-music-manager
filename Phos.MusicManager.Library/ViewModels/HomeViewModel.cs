namespace Phos.MusicManager.Library.ViewModels;

using CommunityToolkit.Mvvm.Input;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.Workspaces;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class HomeViewModel : ViewModelBase, IPage
{
    private readonly NavigationService navigation;
    private readonly WorkspaceService workService;

    public HomeViewModel(NavigationService navigation, WorkspaceService workService)
    {
        this.navigation = navigation;
        this.workService = workService;
    }

    public string Name { get; } = "Home";

    public IEnumerable<Workspace> Projects => this.workService.Projects;

    [RelayCommand]
    private void OpenProject(string projectName)
    {
        this.navigation.NavigateTo(projectName);
    }
}
