namespace Phos.MusicManager.Library.ViewModels.Projects;

using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Commands;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels.Projects.Factories;

#pragma warning disable SA1600 // Elements should be documented
public class ProjectsNavigation : NavigationService
{
    private readonly ILogger? log;
    private readonly ProjectFactory projectFactory;

    public ProjectsNavigation(
        ProjectRepository projectRepo,
        ProjectFactory projectFactory,
        ProjectCommands projectCommands,
        ILogger? log)
        : base(log)
    {
        this.log = log;
        this.projectFactory = projectFactory;

        foreach (var page in projectRepo.List.Select(projectFactory.Create))
        {
            this.Pages.Add(page);
        }

        this.Pages.Add(new AboutViewModel());
        this.Pages.Add(new HomeViewModel(this, projectCommands));

        projectRepo.List.CollectionChanged += this.ProjectList_CollectionChanged;
    }

    private void ProjectList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        try
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newProject = e.NewItems!.Cast<Project>().First();
                    var newProjectPage = this.projectFactory.Create(newProject);
                    this.Pages.Add(newProjectPage);

                    // Dashboard selected item has issues with navigating to new project.
                    // Probably from menu items updating.
                    this.NavigateTo(newProjectPage.Name);
                    break;
                default:
                    throw new Exception("Invalid collection action.");
            }
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to handle collection changed.");
        }
    }
}
