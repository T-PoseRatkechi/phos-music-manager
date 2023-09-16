namespace Phos.MusicManager.Library.Projects;

using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Common.Serializers;

#pragma warning disable SA1600 // Elements should be documented
public class ProjectRepository
{
    private readonly ILogger? log;
    private readonly string projectsDir;
    private readonly IRepository<ProjectPreset, string> presetRepo;
    private readonly ISavable<AppSettings> appSettings;

    public ProjectRepository(
        IRepository<ProjectPreset, string> presetRepo,
        ISavable<AppSettings> appSettings,
        ILogger? log = null)
    {
        this.log = log;
        this.presetRepo = presetRepo;
        this.appSettings = appSettings;
        this.projectsDir = Directory.CreateDirectory(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "projects")).FullName;

        this.LoadProjects();
        this.AddDefaultProjects();
    }

    public ObservableCollection<Project> List { get; } = new();

    public Project Create(ProjectSettings settings)
    {
        var projectFile = Path.Join(this.projectsDir, settings.Name, $"project.phos");
        if (File.Exists(projectFile))
        {
            throw new Exception("Project file already exists.");
        }

        JsonFileSerializer.Serialize(projectFile, settings);
        this.appSettings.Value.ProjectFiles.Add(projectFile);
        this.appSettings.Save();

        var project = new Project(projectFile);
        if (project.Settings.Value.Preset != null)
        {
            if (this.presetRepo.GetById(project.Settings.Value.Preset) is ProjectPreset preset)
            {
                project.Audio.AddTracks(preset.DefaultTracks);
            }
            else
            {
                throw new Exception($"Project preset not found.\nPreset: {project.Settings.Value.Preset}");
            }
        }

        this.List.Add(project);
        return project;
    }

    public void Delete(Project item)
    {
        throw new NotImplementedException();
    }

    public Project? GetById(string id)
    {
        return this.List.FirstOrDefault(x => x.Settings.Value.Name == id);
    }

    public bool Update(Project item)
    {
        throw new NotImplementedException();
    }

    private void LoadProjects()
    {
        var missingProjectFiles = new List<string>();
        foreach (var projectFile in this.appSettings.Value.ProjectFiles)
        {
            if (!File.Exists(projectFile))
            {
                missingProjectFiles.Add(projectFile);
                continue;
            }

            try
            {
                var project = new Project(projectFile);
                this.List.Add(project);
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to load project file.\nFile: {file}", projectFile);
            }
        }

        // Remove missing projects.
        if (missingProjectFiles.Count > 0)
        {
            foreach (var file in missingProjectFiles)
            {
                this.appSettings.Value.ProjectFiles.Remove(file);
            }

            this.appSettings.Save();
        }
    }

    private void AddDefaultProjects()
    {
        foreach (var preset in GamePresets.DefaultPresets)
        {
            if (this.GetById(preset.Name) == null)
            {
                var settings = new ProjectSettings
                {
                    Name = preset.Name,
                    Color = preset.Color,
                    Preset = preset.Name,
                    PostBuild = preset.PostBuild,
                };

                try
                {
                    this.Create(settings);
                }
                catch (Exception ex)
                {
                    this.log?.LogError(ex, "Failed to create default project {name}.", preset.Name);
                }
            }
        }
    }
}
