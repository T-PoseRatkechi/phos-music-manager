namespace Phos.MusicManager.Library.Games;

using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Common;

#pragma warning disable SA1600 // Elements should be documented
public class Game
{
    public Game(string name, string gameFolder)
    {
        this.Name = name;
        this.GameFolder = Directory.CreateDirectory(gameFolder).FullName;
        this.AudioFolder = Directory.CreateDirectory(Path.Join(this.GameFolder, "audio")).FullName;
        this.BuildFolder = Directory.CreateDirectory(Path.Join(this.GameFolder, "build")).FullName;
        this.Settings = new SavableFile<GameSettings>(Path.Join(this.GameFolder, "game-settings.json"));

        var tracksFile = Path.Join(this.AudioFolder, "audio-tracks.json");
        if (!File.Exists(tracksFile))
        {
            var defaultTracksFile = DefaultMusic.GetDefaultMusicFile(name);
            if (defaultTracksFile != null)
            {
                File.Copy(defaultTracksFile, tracksFile, true);
            }
        }

        this.Audio = new(tracksFile);
    }

    public string Name { get; }

    public string GameFolder { get; }

    public string AudioFolder { get; }

    public string BuildFolder { get; }

    public ISavable<GameSettings> Settings { get; }

    public AudioService Audio { get; set; }
}
