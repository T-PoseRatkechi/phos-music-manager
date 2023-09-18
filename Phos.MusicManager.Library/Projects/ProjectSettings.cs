namespace Phos.MusicManager.Library.Projects;

using CommunityToolkit.Mvvm.ComponentModel;

#pragma warning disable SA1601 // Partial elements should be documented
public partial class ProjectSettings : ObservableObject
{
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private ProjectType type = ProjectType.Version1;

    /// <summary>
    /// Gets or sets name.
    /// </summary>
    [ObservableProperty]
    private string name = string.Empty;

    /// <summary>
    /// Gets or sets the project icon color.
    /// </summary>
    [ObservableProperty]
    private string? color;

    /// <summary>
    /// Gets or sets the post build.
    /// </summary>
    [ObservableProperty]
    private string? postBuild;

    /// <summary>
    /// Gets or sets the default project preset.
    /// </summary>
    [ObservableProperty]
    private string? preset;

    /// <summary>
    /// Gets or sets the install path of game target.
    /// </summary>
    [ObservableProperty]
    private string? gameInstallPath;

    /// <summary>
    /// Gets or sets the output directory for build files.
    /// </summary>
    [ObservableProperty]
    private string? outputDir;

    /// <summary>
    /// Gets or sets the app theme to use for project.
    /// </summary>
    [ObservableProperty]
    private string theme = "Dark";
}
