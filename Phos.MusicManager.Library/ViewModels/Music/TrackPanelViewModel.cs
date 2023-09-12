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

/// <summary>
/// Track panel view model.
/// </summary>
#pragma warning disable SA1600 // Elements should be documented
public partial class TrackPanelViewModel : ViewModelBase, IDisposable
{
    private const string NoReplacement = "None";

    private readonly AudioService audioManager;
    private readonly IDialogService dialog;

    private string selectedReplacement;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackPanelViewModel"/> class.
    /// </summary>
    /// <param name="track"></param>
    /// <param name="audioManager"></param>
    /// <param name="encoders">List of encoders.</param>
    /// <param name="dialog"></param>
    /// <param name="closeCommand"></param>
    public TrackPanelViewModel(
        AudioTrack track,
        AudioService audioManager,
        string[] encoders,
        IDialogService dialog,
        ICommand closeCommand)
    {
        this.audioManager = audioManager;
        this.dialog = dialog;

        this.Track = track;
        this.Encoders = encoders;
        this.SelectedEncoder = track.Encoder;
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

    public string SelectedEncoder
    {
        get => this.Track.Encoder;
        set => this.Track.Encoder = value;
    }

    public void Dispose()
    {
        this.Track.PropertyChanged -= this.Track_PropertyChanged;
        this.Track.Loop.PropertyChanged -= this.Track_PropertyChanged;
        GC.SuppressFinalize(this);
    }

    private void Track_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.audioManager.SaveTracks();

        if (e.PropertyName == nameof(this.Track.ReplacementFile) || e.PropertyName == nameof(this.Track.Loop.Enabled))
        {
            this.OnPropertyChanged(nameof(this.LoopInputEnabled));
        }
    }

    [RelayCommand]
    private async Task SelectReplacementFile()
    {
        var replacementFile = await this.dialog.OpenFileSelect("Select Replacement File...");
        if (replacementFile != null)
        {
            this.Replacements.Add(replacementFile);
            this.SelectedReplacement = replacementFile;
        }
    }

    [RelayCommand]
    private void Delete()
    {
        this.audioManager.Tracks.Remove(this.Track);
    }
}
