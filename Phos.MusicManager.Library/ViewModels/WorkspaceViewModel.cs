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
using Phos.MusicManager.Library.ViewModels.Music;
using Phos.MusicManager.Library.ViewModels.Workspaces.Dialogs;
using Phos.MusicManager.Library.Workspaces;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
public partial class WorkspaceViewModel : ViewModelBase, IPage
{
    private readonly Workspace workspace;
    private readonly AudioBuilder audioBuilder;
    private readonly MusicFactory musicFactory;
    private readonly IDialogService dialog;
    private readonly ILogger? log;

    [ObservableProperty]
    private TrackPanelViewModel? trackPanel;
    private AudioTrack? selectedTrack;

    public WorkspaceViewModel(
        Workspace workspace,
        AudioBuilder audioBuilder,
        MusicFactory musicFactory,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.log = log;
        this.workspace = workspace;
        this.audioBuilder = audioBuilder;
        this.musicFactory = musicFactory;
        this.dialog = dialog;
    }

    public string Name => this.workspace.Settings.Value.Name;

    public ObservableCollection<AudioTrack> Tracks => this.workspace.Audio.Tracks;

    public string? Color => this.workspace.Settings.Value.Color;

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
            var outputDir = this.workspace.Settings.Value.OutputDir ?? this.workspace.BuildFolder;
            await this.audioBuilder.Build(this.workspace.Audio.Tracks, outputDir);
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to build output for project {project}.", this.workspace.Settings.Value.Name);
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
            this.workspace.Audio.Tracks.Add(newTrack);
        }
    }

    [RelayCommand]
    private async Task OpenSettings()
    {
        var settings = new ProjectSettingsViewModel(this.workspace.Settings, this.dialog);
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

        this.TrackPanel = this.musicFactory.CreateTrackPanel(this.SelectedTrack, this.workspace.Audio, this.CloseTrackPanelCommand);
    }
}
