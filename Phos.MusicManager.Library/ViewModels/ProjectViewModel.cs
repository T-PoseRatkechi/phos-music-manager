namespace Phos.MusicManager.Library.ViewModels;

using System.Collections.ObjectModel;
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
using Phos.MusicManager.Library.ViewModels.Projects.Dialogs;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class ProjectViewModel : ViewModelBase, IPage
{
    private readonly AudioBuilder audioBuilder;
    private readonly MusicFactory musicFactory;
    private readonly IDialogService dialog;
    private readonly ILogger? log;

    [ObservableProperty]
    private TrackPanelViewModel? trackPanel;
    private AudioTrack? selectedTrack;

    public ProjectViewModel(
        Project project,
        AudioBuilder audioBuilder,
        MusicFactory musicFactory,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.log = log;
        this.audioBuilder = audioBuilder;
        this.musicFactory = musicFactory;
        this.dialog = dialog;

        this.Project = project;
    }

    public Project Project { get; }

    public string Name => this.Project.Settings.Value.Name;

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
        var settings = new ProjectSettingsViewModel(this.Project.Settings, this.dialog);
        await this.dialog.OpenDialog(settings);
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
