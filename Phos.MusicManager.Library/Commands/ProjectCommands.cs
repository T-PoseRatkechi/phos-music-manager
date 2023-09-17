namespace Phos.MusicManager.Library.Commands;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels.Projects.Factories;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class ProjectCommands : ObservableObject
{
    private readonly ProjectRepository projectRepository;
    private readonly CreateProjectFactory createProjectFactory;
    private readonly IDialogService dialog;
    private readonly ILogger? log;

    public ProjectCommands(
        ProjectRepository projectRepository,
        CreateProjectFactory createProjectFactory,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.projectRepository = projectRepository;
        this.createProjectFactory = createProjectFactory;
        this.dialog = dialog;
        this.log = log;
    }

    public IRelayCommand Create_NewProjectCommand(Func<bool>? canExecute = null)
        => canExecute == null ? new AsyncRelayCommand(this.NewProject) : new AsyncRelayCommand(this.NewProject, canExecute);

    private async Task NewProject()
    {
        var createProject = this.createProjectFactory.Create();
        var projectSettings = await this.dialog.OpenDialog<ProjectSettings>(createProject);
        if (projectSettings == null)
        {
            return;
        }

        try
        {
            var project = this.projectRepository.Create(projectSettings);
            var projectIconFile = Path.Join(project.ProjectFolder, "icon.png");

            if (createProject.Icon != null && createProject.Icon is string iconFile)
            {
                File.Copy(iconFile, projectIconFile, true);
            }
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to create new project.");
        }
    }
}
