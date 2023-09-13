namespace Phos.MusicManager.Library.ViewModels.Music;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.ViewModels.Music.Dialogs;

/// <summary>
/// Track panel view model.
/// </summary>
#pragma warning disable SA1600 // Elements should be documented
public partial class TrackPanelViewModel : ViewModelBase, IDisposable
{
    private const string NoReplacement = "None";

    private readonly AudioService audioManager;
    private readonly LoopService loopService;
    private readonly IDialogService dialog;

    private string selectedReplacement;

    public TrackPanelViewModel(
        AudioTrack track,
        AudioService audioManager,
        LoopService loopService,
        string[] encoders,
        IDialogService dialog,
        ICommand closeCommand)
    {
        this.audioManager = audioManager;
        this.loopService = loopService;
        this.dialog = dialog;

        this.Track = track;
        this.Encoders = encoders;
        this.CloseCommand = closeCommand;

        // Set current replacement selection.
        if (track.ReplacementFile != null)
        {
            this.Replacements.Add(track.ReplacementFile);
            this.selectedReplacement = track.ReplacementFile;
        }
        else
        {
            this.selectedReplacement = NoReplacement;
        }

        // Save tracks on changes made.
        this.Track.PropertyChanged += this.Track_PropertyChanged;
        this.Track.Loop.PropertyChanged += this.Track_PropertyChanged;
        this.Track.Loop.PropertyChanged += this.Loop_PropertyChanged;
    }

    public AudioTrack Track { get; }

    public ICommand CloseCommand { get; }

    public bool LoopInputEnabled => this.Track.ReplacementFile != null && this.Track.Loop.Enabled;

    public ObservableCollection<string> Replacements { get; } = new() { NoReplacement };

    public string SelectedReplacement
    {
        get => this.selectedReplacement;
        set
        {
            this.SetProperty(ref this.selectedReplacement, value);

            // Push selection to track.
            if (this.selectedReplacement == NoReplacement)
            {
                this.Track.ReplacementFile = null;
            }
            else
            {
                this.Track.ReplacementFile = this.selectedReplacement;
            }
        }
    }

    public string[] Encoders { get; }

    public void Dispose()
    {
        this.Track.PropertyChanged -= this.Track_PropertyChanged;
        this.Track.Loop.PropertyChanged -= this.Track_PropertyChanged;
        this.Track.Loop.PropertyChanged -= this.Loop_PropertyChanged;
        GC.SuppressFinalize(this);
    }

    [RelayCommand]
    private async Task SelectReplacementFile()
    {
        var replacementFile = await this.dialog.OpenFileSelect("Select Replacement File...");
        if (replacementFile != null)
        {
            var savedLoop = this.loopService.GetLoop(replacementFile);
            if (savedLoop != null)
            {
                this.Track.Loop.Enabled = savedLoop.Enabled;
                this.Track.Loop.StartSample = savedLoop.StartSample;
                this.Track.Loop.EndSample = savedLoop.EndSample;
            }
            else
            {
                this.Track.Loop.Enabled = true;
                this.Track.Loop.StartSample = 0;
                this.Track.Loop.EndSample = 0;
            }

            this.Replacements.Add(replacementFile);
            this.SelectedReplacement = replacementFile;
        }
    }

    [RelayCommand]
    private async Task Edit()
    {
        var editTrack = new AddTrackViewModel(this.Encoders, this.Track);
        var updatedTrack = await this.dialog.OpenDialog<AudioTrack>(editTrack);

        if (updatedTrack != null)
        {
            this.Track.Name = updatedTrack.Name;
            this.Track.Category = updatedTrack.Category;
            this.Track.Tags = updatedTrack.Tags;
            this.Track.OutputPath = updatedTrack.OutputPath;
            this.Track.Encoder = updatedTrack.Encoder;
        }
    }

    [RelayCommand]
    private void Delete()
    {
        this.audioManager.Tracks.Remove(this.Track);
    }

    private void Track_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Save tracks on changes.
        this.audioManager.SaveTracks();

        // Update loop input enabled state.
        if (e.PropertyName == nameof(this.Track.ReplacementFile) || e.PropertyName == nameof(this.Track.Loop.Enabled))
        {
            this.OnPropertyChanged(nameof(this.LoopInputEnabled));
        }
    }

    private void Loop_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Save loop settings for replacement file on changes.
        if (this.Track.ReplacementFile != null)
        {
            this.loopService.SaveLoop(this.Track.ReplacementFile, this.Track.Loop);
        }
    }
}
