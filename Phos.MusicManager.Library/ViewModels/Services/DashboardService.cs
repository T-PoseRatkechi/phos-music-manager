namespace Phos.MusicManager.Library.ViewModels.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.ViewModels.Music;
using Phos.MusicManager.Library.Workspaces;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class DashboardService : ObservableObject
{
    private readonly ILogger? log;
    private readonly WorkspaceService workService;
    private readonly AudioBuilder audioBuilder;
    private readonly MusicFactory musicFactory;
    private readonly IDialogService dialog;

    public DashboardService(
        WorkspaceService workService,
        AudioBuilder audioBuilder,
        MusicFactory musicFactory,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.log = log;
        this.workService = workService;
        this.audioBuilder = audioBuilder;
        this.musicFactory = musicFactory;
        this.dialog = dialog;

        this.Navigation = new NavigationService();

        var home = new HomeViewModel(this.Navigation, workService);
        var about = new AboutViewModel();
        var workPages = this.GetWorkPages();

        this.Navigation.Pages.Add(home);
        this.Navigation.Pages.Add(about);
        foreach (var page in workPages)
        {
            this.Navigation.Pages.Add(page);
        }
    }

    public NavigationService Navigation { get; }

    private WorkspaceViewModel[] GetWorkPages()
    {
        return this.workService.Projects.Select(x => new WorkspaceViewModel(x, this.audioBuilder, this.musicFactory, this.dialog, this.log)).ToArray();
    }
}
