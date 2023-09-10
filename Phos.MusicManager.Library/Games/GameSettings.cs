namespace Phos.MusicManager.Library.Games;

#pragma warning disable SA1600 // Elements should be documented
public class GameSettings
{
    public string Theme { get; set; } = "Dark";

    public string? InstallPath { get; set; }

    public string? OutputDir { get; set; }
}
