namespace Phos.MusicManager.Library.ViewModels.Projects.Factories;

using Phos.MusicManager.Library.Commands;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels.Projects.Dialogs;

#pragma warning disable SA1600 // Elements should be documented
public class CreateProjectFactory
{
    private readonly ProjectRepository projectRepo;
    private readonly ProjectPresetRepository presetRepo;
    private readonly CreatePresetCommand createPresetCommand;
    private readonly IDialogService dialog;

    public CreateProjectFactory(
        ProjectRepository projectRepo,
        ProjectPresetRepository presetRepo,
        CreatePresetCommand createPresetCommand,
        IDialogService dialog)
    {
        this.projectRepo = projectRepo;
        this.presetRepo = presetRepo;
        this.createPresetCommand = createPresetCommand;
        this.dialog = dialog;
    }

    public CreateProjectViewModel Create(Project? existingProject = null)
    {
        var createProject = new CreateProjectViewModel(this.projectRepo, this.presetRepo, this.createPresetCommand, this.dialog, existingProject);
        return createProject;
    }
}
