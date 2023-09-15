namespace Phos.MusicManager.Library.Workspaces;

using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Common.Serializers;

#pragma warning disable SA1600 // Elements should be documented
public class WorkspaceService
{
    private readonly string projectsDir;
    private readonly string presetsDir;

    private readonly ILogger? log;
    private readonly ISavable<AppSettings> appSettings;

    public WorkspaceService(ISavable<AppSettings> appSettings, ILogger? log = null)
    {
        this.log = log;
        this.appSettings = appSettings;

        this.projectsDir = Directory.CreateDirectory(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "projects")).FullName;
        this.presetsDir = Directory.CreateDirectory(Path.Join(this.projectsDir, "presets")).FullName;

        this.Projects = this.GetProjects();
        this.AddDefaultProjects();

        this.Presets = this.GetPresets();
    }

    public ObservableCollection<Workspace> Projects { get; }

    public ObservableCollection<ProjectPreset> Presets { get; }

    public Workspace CreateWorkspace(string projectFile, ProjectPreset? preset = null)
    {
        var project = new SavableFile<ProjectSettings>(projectFile);
        var projectDir = Path.GetDirectoryName(projectFile)!;

        var audioTracksFile = Path.Join(projectDir, "audio", "audio-tracks.json");
        var audioTracks = new SavableFile<ObservableCollection<AudioTrack>>(audioTracksFile);
        var audioService = new AudioService(audioTracks);

        var workspace = new Workspace(projectDir, project, audioService);

        if (preset != null)
        {
            workspace.Settings.Value.Name = preset.Name;
            workspace.Settings.Value.Game = preset.Game;
            workspace.Settings.Value.Color = preset.Color;
            workspace.Settings.Value.Preset = preset.Name;
            workspace.Settings.Value.PostBuild = preset.PostBuild;
            workspace.Settings.Save();
            workspace.Audio.AddTracks(preset.DefaultTracks);
        }

        if (!this.appSettings.Value.ProjectFiles.Contains(projectFile))
        {
            this.appSettings.Value.ProjectFiles.Add(projectFile);
            this.appSettings.Save();
        }

        return workspace;
    }

    private ObservableCollection<Workspace> GetProjects()
    {
        var currentProjects = new ObservableCollection<Workspace>();
        var removedProjects = new List<string>();

        // Load projects.
        foreach (var projectFile in this.appSettings.Value.ProjectFiles)
        {
            if (!File.Exists(projectFile))
            {
                removedProjects.Add(projectFile);
                continue;
            }

            try
            {
                currentProjects.Add(this.CreateWorkspace(projectFile));
            }
            catch (Exception ex)
            {
                this.log?.LogWarning(ex, "Failed to load project.\nFile: {file}", projectFile);
            }
        }

        if (removedProjects.Count > 0)
        {
            foreach (var project in removedProjects)
            {
                this.appSettings.Value.ProjectFiles.Remove(project);
            }

            this.appSettings.Save();
        }

        return currentProjects;
    }

    private void AddDefaultProjects()
    {
        var defaultProjects = new string[] { Constants.P4G_PC_64, Constants.P5R_PC, Constants.P3P_PC };
        foreach (var defaultProject in defaultProjects)
        {
            // Default project missing.
            if (this.Projects.FirstOrDefault(x => x.Settings.Value.Name == defaultProject) == null)
            {
                var projectFile = Path.Join(this.projectsDir, defaultProject, "project.phos");
                switch (defaultProject)
                {
                    case Constants.P4G_PC_64:
                        this.Projects.Add(this.CreateWorkspace(projectFile, GamePresets.P4G_PC_64));
                        break;
                    case Constants.P5R_PC:
                        this.Projects.Add(this.CreateWorkspace(projectFile, GamePresets.P5R_PC));
                        break;
                    case Constants.P3P_PC:
                        this.Projects.Add(this.CreateWorkspace(projectFile, GamePresets.P3P_PC));
                        break;
                    default:
                        this.log?.LogWarning("Unknown default game project {game}.", defaultProject);
                        break;
                }
            }
        }
    }

    private ObservableCollection<ProjectPreset> GetPresets()
    {
        var presets = new ObservableCollection<ProjectPreset>()
        {
            GamePresets.P4G_PC_64,
            GamePresets.P5R_PC,
            GamePresets.P3P_PC,
        };

        // Get presets from files.
        foreach (var file in Directory.EnumerateFiles(this.presetsDir, "*.json", SearchOption.AllDirectories))
        {
            try
            {
                var preset = JsonFileSerializer.Deserialize<ProjectPreset>(file) ?? throw new ArgumentException();
                presets.Add(preset);
            }
            catch (Exception ex)
            {
                this.log?.LogWarning(ex, "Failed to load project preset.\nFile: {file}", file);
            }
        }

        return presets;
    }
}
