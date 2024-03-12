namespace Phos.MusicManager.Library.Projects;

using Phos.MusicManager.Library.Audio;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1310 // Field names should not contain underscore
public static class GamePresets
{
    public static readonly ProjectPreset P4G_PC_64 = new()
    {
        Name = Constants.P4G_PC_64,
        Color = "#FFD700",
        DefaultTracks = DefaultMusic.GetDefaultMusic(Constants.P4G_PC_64),
        Icon = File.Exists(GetIconFile(Constants.P4G_PC_64)) ? File.ReadAllBytes(GetIconFile(Constants.P4G_PC_64)) : null,
    };

    public static readonly ProjectPreset P5R_PC = new()
    {
        Name = Constants.P5R_PC,
        Color = "#C62828",
        DefaultTracks = DefaultMusic.GetDefaultMusic(Constants.P5R_PC),
        Icon = File.Exists(GetIconFile(Constants.P5R_PC)) ? File.ReadAllBytes(GetIconFile(Constants.P5R_PC)) : null,
    };

    public static readonly ProjectPreset P3P_PC = new()
    {
        Name = Constants.P3P_PC,
        Color = "#1976D2",
        DefaultTracks = DefaultMusic.GetDefaultMusic(Constants.P3P_PC),
        Icon = File.Exists(GetIconFile(Constants.P3P_PC)) ? File.ReadAllBytes(GetIconFile(Constants.P3P_PC)) : null,
    };

    public static readonly ProjectPreset P3R_PC = new()
    {
        Name = Constants.P3R_PC,
        Color = "#1976D2",
        DefaultTracks = DefaultMusic.GetDefaultMusic(Constants.P3R_PC),
        Icon = File.Exists(GetIconFile(Constants.P3R_PC)) ? File.ReadAllBytes(GetIconFile(Constants.P3R_PC)) : null,
    };

    public static readonly ProjectPreset[] DefaultPresets = new ProjectPreset[]
    {
        P4G_PC_64,
        P5R_PC,
        P3P_PC,
        P3R_PC,
    };

    private static string GetIconFile(string game) =>
        Path.Join(AppDomain.CurrentDomain.BaseDirectory, "resources", "icons", $"{game}.png");
}
