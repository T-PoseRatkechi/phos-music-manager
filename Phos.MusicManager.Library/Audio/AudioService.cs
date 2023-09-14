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
    /// <param name="tracks">Audio tracks.</param>
    public AudioService(ISavable<ObservableCollection<AudioTrack>> tracks)
    {
        this.tracks = tracks;
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

    /// <summary>
    /// Add multiple tracks.
    /// </summary>
    /// <param name="tracks">Audio tracks to add.</param>
    public void AddTracks(IEnumerable<AudioTrack> tracks)
    {
        this.Tracks.CollectionChanged -= this.Tracks_CollectionChanged;

        foreach (var track in tracks)
        {
            this.Tracks.Add(track);
        }

        this.SaveTracks();
        this.Tracks.CollectionChanged += this.Tracks_CollectionChanged;
    }

    private void Tracks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.tracks.Save();
    }
}
