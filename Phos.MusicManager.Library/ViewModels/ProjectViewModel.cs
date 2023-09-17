namespace Phos.MusicManager.Library.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels.Music;
using Phos.MusicManager.Library.ViewModels.Projects.Factories;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class ProjectViewModel : ViewModelBase, IPage
{
    private readonly AudioBuilder audioBuilder;
    private readonly MusicFactory musicFactory;
    private readonly CreateProjectFactory createProjectFactory;
    private readonly IDialogService dialog;
    private readonly ILogger? log;

    [ObservableProperty]
    private TrackPanelViewModel? trackPanel;
    private AudioTrack? selectedTrack;

    public ProjectViewModel(
        Project project,
        AudioBuilder audioBuilder,
        MusicFactory musicFactory,
        CreateProjectFactory createProjectFactory,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.log = log;
        this.audioBuilder = audioBuilder;
        this.musicFactory = musicFactory;
        this.createProjectFactory = createProjectFactory;
        this.dialog = dialog;

        this.Project = project;
    }

    public Project Project { get; }

    public string Name => this.Project.Settings.Value.Id;

    public string? IconFile
    {
        get
        {
            var iconFile = Path.Join(this.Project.ProjectFolder, "icon.png");
            if (File.Exists(iconFile))
            {
                return iconFile;
            }

            return null;
        }
    }

    public AudioTrack? SelectedTrack
    {
        get => this.selectedTrack;
        set
        {
            this.SetProperty(ref this.selectedTrack, value);
            this.UpdateTrackPanel();
        }
    }

    private bool CanBuild { get; set; } = true;

    [RelayCommand(CanExecute = nameof(this.CanBuild))]
    private async Task Build()
    {
        try
        {
            this.CanBuild = false;
            var outputDir = this.Project.Settings.Value.OutputDir ?? this.Project.BuildFolder;
            await this.audioBuilder.Build(this.Project.Audio.Tracks, outputDir);
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to build output for project {project}.", this.Project.Settings.Value.Name);
        }
        finally
        {
            this.CanBuild = true;
        }
    }

    [RelayCommand]
    private async Task AddTrack()
    {
        var addTrack = this.musicFactory.CreateAddTrack();
        var newTrack = await this.dialog.OpenDialog<AudioTrack>(addTrack);
        if (newTrack != null)
        {
            this.Project.Audio.Tracks.Add(newTrack);
        }
    }

    [RelayCommand]
    private async Task OpenSettings()
    {
        var editProject = this.createProjectFactory.Create(this.Project);
        var newSettings = await this.dialog.OpenDialog<ProjectSettings>(editProject);
        if (newSettings == null)
        {
            return;
        }

        // Update settings.
        this.Project.Settings.Value.Name = newSettings.Name;
        this.Project.Settings.Value.Color = newSettings.Color;
        this.Project.Settings.Value.Preset = newSettings.Preset;
        this.Project.Settings.Value.PostBuild = newSettings.PostBuild;
        this.Project.Settings.Value.GameInstallPath = newSettings.GameInstallPath;
        this.Project.Settings.Value.OutputDir = newSettings.OutputDir;
        this.Project.Settings.Value.Theme = newSettings.Theme;
        this.Project.Settings.Save();

        var projectIconFile = Path.Join(this.Project.ProjectFolder, "icon.png");

        // Icon was removed.
        if (File.Exists(projectIconFile) && editProject.Icon == null)
        {
            File.Delete(projectIconFile);
            this.OnPropertyChanged(nameof(this.IconFile));
        }

        // New icon was selected.
        else if (editProject.Icon is string iconFile && iconFile != projectIconFile)
        {
            File.Copy(iconFile, projectIconFile, true);
            this.OnPropertyChanged(nameof(this.IconFile));
        }
    }

    [RelayCommand]
    private void CloseTrackPanel()
    {
        this.SelectedTrack = null;
    }

    private void UpdateTrackPanel()
    {
        this.TrackPanel?.Dispose();
        if (this.SelectedTrack == null)
        {
            this.TrackPanel = null;
            return;
        }

        this.TrackPanel = this.musicFactory.CreateTrackPanel(this.SelectedTrack, this.Project.Audio, this.CloseTrackPanelCommand);
    }
}
