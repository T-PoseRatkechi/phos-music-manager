namespace Phos.MusicManager.Library.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Games;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.ViewModels.Music;

/// <summary>
/// Game hub view model.
/// </summary>
public partial class GameHubViewModel : ViewModelBase, IPage
{
    private readonly Game game;
    private readonly TrackPanelFactory trackPanelFactory;

    [ObservableProperty]
    private TrackPanelViewModel? trackPanel;
    private AudioTrack? selectedTrack;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameHubViewModel"/> class.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="trackPanelFactory"></param>
    public GameHubViewModel(Game game, TrackPanelFactory trackPanelFactory)
    {
        this.game = game;
        this.trackPanelFactory = trackPanelFactory;
    }

    /// <inheritdoc/>
    public string Name => this.game.Name;

    /// <summary>
    /// Gets game audio tracks.
    /// </summary>
    public ObservableCollection<AudioTrack> Tracks => this.game.Audio.Tracks;

    /// <summary>
    /// Gets or sets selected track.
    /// </summary>
    public AudioTrack? SelectedTrack
    {
        get => this.selectedTrack;
        set
        {
            this.SetProperty(ref this.selectedTrack, value);
            this.UpdateTrackPanel();
        }
    }

    private void UpdateTrackPanel()
    {
        this.TrackPanel?.Dispose();
        if (this.SelectedTrack == null)
        {
            this.TrackPanel = null;
            return;
        }

        this.TrackPanel = this.trackPanelFactory.Create(this.SelectedTrack, this.game.Audio, this.CloseTrackPanelCommand);
    }

    [RelayCommand]
    private void CloseTrackPanel()
    {
        this.SelectedTrack = null;
    }
}
