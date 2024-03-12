namespace Phos.MusicManager.Library.Metadata;

public class GameMusic
{
    public string Game { get; set; }

    public int Version { get; set; }

    public string DefaultBaseReplacementPath { get; set; } = string.Empty;

    public string? DefaultEncoder { get; set; }

    public List<Song> Songs { get; set; } = new();
}