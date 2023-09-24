namespace Phos.MusicManager.Library.Audio;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;

/// <summary>
/// Audio service.
/// </summary>
public class AudioService
{
    private readonly ILogger? log;
    private readonly Project project;
    private readonly ISavable<ObservableCollection<AudioTrack>> tracks;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioService"/> class.
    /// </summary>
    /// <param name="tracks">Audio tracks.</param>
    public AudioService(
        Project project,
        ISavable<ObservableCollection<AudioTrack>> tracks,
        ILogger? log = null)
    {
        this.log = log;
        this.project = project;
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
    /// Removes an audio track along with any existing output file.
    /// </summary>
    /// <param name="track"></param>
    public void RemoveTrack(AudioTrack track)
    {
        var projectBuildFile = Path.Join(this.project.BuildFolder, track.OutputPath);
        var outputBuildFile = this.project.Settings.Value.OutputDir != null
            ? Path.Join(this.project.Settings.Value.OutputDir, track.OutputPath) : null;

        try
        {
            File.Delete(projectBuildFile);
            if (outputBuildFile != null)
            {
                File.Delete(outputBuildFile);
            }

            this.Tracks.Remove(track);
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to remove audio track output file.\nFile: {file}", outputBuildFile ?? projectBuildFile);
        }
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
