namespace Phos.MusicManager.Library.Projects;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class Project : ObservableObject
{
    public Project(string projectFile)
    {
        this.ProjectFile = projectFile;
        this.Settings = new SavableFile<ProjectSettings>(projectFile);
        this.ProjectFolder = Directory.CreateDirectory(Path.GetDirectoryName(projectFile)!).FullName;
        this.AudioFolder = Directory.CreateDirectory(Path.Join(this.ProjectFolder, "audio")).FullName;
        this.BuildFolder = Directory.CreateDirectory(Path.Join(this.ProjectFolder, "build")).FullName;

        var audioTracksFile = Path.Join(this.AudioFolder, "audio-tracks.json");
        var audioTracks = new SavableFile<ObservableCollection<AudioTrack>>(audioTracksFile);
        this.Audio = new(audioTracks);
    }

    public string ProjectFile { get; }

    public string ProjectFolder { get; }

    public string AudioFolder { get; }

    public string BuildFolder { get; }

    public ISavable<ProjectSettings> Settings { get; }

    public AudioService Audio { get; }
}
