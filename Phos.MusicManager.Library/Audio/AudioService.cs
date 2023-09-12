namespace Phos.MusicManager.Library.Audio;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;

/// <summary>
/// Audio service.
/// </summary>
public class AudioService
{
    private readonly ISavable<ObservableCollection<AudioTrack>> tracks;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioService"/> class.
    /// </summary>
    /// <param name="tracksFile"></param>
    public AudioService(string tracksFile)
    {
        this.tracks = new SavableFile<ObservableCollection<AudioTrack>>(tracksFile);
        this.Tracks.CollectionChanged += this.Tracks_CollectionChanged;
    }

    /// <summary>
    /// Gets audio tracks.
    /// </summary>
    public ObservableCollection<AudioTrack> Tracks => this.tracks.Value;

    /// <summary>
    /// Save current audio tracks data.
    /// </summary>
    public void SaveTracks()
    {
        this.tracks.Save();
    }

    private void Tracks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.tracks.Save();
    }
}
