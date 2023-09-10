namespace Phos.MusicManager.Library.Games;

using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Common;

#pragma warning disable SA1600 // Elements should be documented
public class Game
{
    public Game(string name, ISavable<GameSettings> settings)
    {
        this.Name = name;
        this.Settings = settings;
    }

    public string Name { get; set; }

    public ISavable<GameSettings> Settings { get; set; }
}
