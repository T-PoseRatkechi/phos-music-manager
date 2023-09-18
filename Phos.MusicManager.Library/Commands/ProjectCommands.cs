namespace Phos.MusicManager.Library.Commands;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels.Dialogs;
using Phos.MusicManager.Library.ViewModels.Projects.Factories;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class ProjectCommands : ObservableObject
{
    private readonly ProjectRepository projectRepository;
    private readonly PortableProjectConverter projectConverter;
    private readonly CreateProjectFactory createProjectFactory;
    private readonly IDialogService dialog;
    private readonly ILogger? log;

    public ProjectCommands(
        ProjectRepository projectRepository,
        PortableProjectConverter projectConverter,
        CreateProjectFactory createProjectFactory,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.projectRepository = projectRepository;
        this.projectConverter = projectConverter;
        this.createProjectFactory = createProjectFactory;
        this.dialog = dialog;
        this.log = log;
    }

    [RelayCommand]
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

    [RelayCommand]
    private async Task OpenProject()
    {
        var projectFile = await this.dialog.OpenFileSelect("Open Project", "Project File|*.phos");
        if (projectFile == null)
        {
            return;
        }

        try
        {
            var project = new Project(projectFile);
            if (project.Settings.Value.Type == ProjectType.Portable1)
            {
                var confirmDialog = new ConfirmViewModel()
                {
                    Title = "Open Project",
                    Subtitle = "Convert Portable Project",
                    BodyText = "Opening the portable project will convert it to a normal project. Continue?",
                    ConfirmText = "Convert Project",
                    CancelText = "Cancel",
                };

                var confirmConvert = await this.dialog.OpenDialog<bool>(confirmDialog);
                if (!confirmConvert)
                {
                    return;
                }

                // Convert project.
                this.projectConverter.Convert(project);
            }

            this.projectRepository.Add(project);
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to open project.");
        }
    }
}
