namespace Phos.MusicManager.Library.ViewModels.Projects.Factories;

using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels.Music;

#pragma warning disable SA1600 // Elements should be documented
public class ProjectFactory
{
    private readonly AudioBuilder audioBuilder;
    private readonly MusicFactory musicFactory;
    private readonly CreateProjectFactory createProjectFactory;
    private readonly IDialogService dialog;
    private readonly ILogger? log;

    public ProjectFactory(
        AudioBuilder audioBuilder,
        MusicFactory musicFactory,
        CreateProjectFactory createProjectFactory,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.audioBuilder = audioBuilder;
        this.musicFactory = musicFactory;
        this.createProjectFactory = createProjectFactory;
        this.dialog = dialog;
        this.log = log;
    }

    public ProjectViewModel Create(Project project)
    {
        var projectViewModel = new ProjectViewModel(project, this.audioBuilder, this.musicFactory, this.createProjectFactory, this.dialog, this.log);
        return projectViewModel;
    }
}
