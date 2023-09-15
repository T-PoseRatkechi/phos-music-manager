namespace Phos.MusicManager.Library.Workspaces;

using Phos.MusicManager.Library.Audio;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1310 // Field names should not contain underscore
public static class GamePresets
{
    public static readonly ProjectPreset P4G_PC_64 = new()
    {
        Name = Constants.P4G_PC_64,
        Game = Constants.P4G_PC_64,
        Color = "#FFD700",
        DefaultTracks = DefaultMusic.GetDefaultMusic(Constants.P4G_PC_64),
    };

    public static readonly ProjectPreset P5R_PC = new()
    {
        Name = Constants.P5R_PC,
        Game = Constants.P5R_PC,
        Color = "#C62828",
        DefaultTracks = DefaultMusic.GetDefaultMusic(Constants.P5R_PC),
    };

    public static readonly ProjectPreset P3P_PC = new()
    {
        Name = Constants.P3P_PC,
        Game = Constants.P3P_PC,
        Color = "#1976D2",
        DefaultTracks = DefaultMusic.GetDefaultMusic(Constants.P3P_PC),
    };
}
