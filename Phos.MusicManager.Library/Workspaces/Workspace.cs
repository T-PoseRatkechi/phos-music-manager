namespace Phos.MusicManager.Library.Workspaces;

using CommunityToolkit.Mvvm.ComponentModel;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Common;

/// <summary>
/// Workspace project.
/// </summary>
public partial class Workspace : ObservableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Workspace"/> class.
    /// </summary>
    /// <param name="workspaceFolder">Workspace folder.</param>
    /// <param name="settings">Workspace project settings.</param>
    /// <param name="audio">Workspace audio.</param>
    public Workspace(string workspaceFolder, ISavable<ProjectSettings> settings, AudioService audio)
    {
        this.WorkspaceFolder = workspaceFolder;
        this.Settings = settings;
        this.Audio = audio;
        this.AudioFolder = Directory.CreateDirectory(Path.Join(this.WorkspaceFolder, "audio")).FullName;
        this.BuildFolder = Directory.CreateDirectory(Path.Join(this.WorkspaceFolder, "build")).FullName;
    }

    /// <summary>
    /// Gets project settings.
    /// </summary>
    public ISavable<ProjectSettings> Settings { get; }

    /// <summary>
    /// Gets workspace folder.
    /// </summary>
    public string WorkspaceFolder { get; }

    /// <summary>
    /// Gets workspace audio folder.
    /// </summary>
    public string AudioFolder { get; }

    /// <summary>
    /// Gets workspace build folder.
    /// </summary>
    public string BuildFolder { get; }

    /// <summary>
    /// Gets workspace audio.
    /// </summary>
    public AudioService Audio { get; }
}
