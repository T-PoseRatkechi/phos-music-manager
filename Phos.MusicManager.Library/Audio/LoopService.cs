namespace Phos.MusicManager.Library.Audio;

using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common.Serializers;

/// <summary>
/// Loop service.
/// </summary>
public class LoopService
{
    private readonly DirectoryInfo loopsFolder = new(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "audio", "loops"));

    /// <summary>
    /// Initializes a new instance of the <see cref="LoopService"/> class.
    /// </summary>
    public LoopService()
    {
        this.loopsFolder.Create();
    }

    /// <summary>
    /// Gets saved loop settings for the given file.
    /// </summary>
    /// <param name="file">File to get loop for.</param>
    /// <returns>Saved loop settings or <c>null</c>.</returns>
    public Loop? GetLoop(string file)
    {
        var loopFile = this.GetLoopFile(file);
        if (loopFile == null)
        {
            return null;
        }

        var loop = JsonFileSerializer.Deserialize<Loop>(loopFile);
        return loop;
    }

    /// <summary>
    /// Saves loop settings for the given file.
    /// </summary>
    /// <param name="file">File to save loop for.</param>
    /// <param name="loop">Loop settings.</param>
    /// <param name="outputLoopFile">Optional output loop file.</param>
    public void SaveLoop(string file, Loop loop, string? outputLoopFile = null)
    {
        JsonFileSerializer.Serialize(this.SavedLoopFile(file), loop);
        if (outputLoopFile != null)
        {
            JsonFileSerializer.Serialize(outputLoopFile, loop);
        }
    }

    private string? GetLoopFile(string inputFile)
    {
        // Loop settings saved by app.
        var loopFile = this.SavedLoopFile(inputFile);
        if (File.Exists(loopFile))
        {
            return loopFile;
        }

        // Loop settings relative to file.
        var relLoopFile = $"{inputFile}.json";
        if (File.Exists(relLoopFile))
        {
            return relLoopFile;
        }

        return null;
    }

    private string SavedLoopFile(string audioFile) => Path.Join(this.loopsFolder.FullName, $"{Path.GetFileName(audioFile)}.json");
}
