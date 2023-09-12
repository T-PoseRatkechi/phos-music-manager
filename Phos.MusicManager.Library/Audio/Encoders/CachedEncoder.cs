namespace Phos.MusicManager.Library.Audio.Encoders;

using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Common.Serializers;

/// <summary>
/// Encoder wrapper that caches encoded files.
/// </summary>
public class CachedEncoder : IEncoder
{
    private readonly ILogger? log;
    private readonly IEncoder encoder;
    private readonly string cachedFolder;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedEncoder"/> class.
    /// </summary>
    /// <param name="encoder"></param>
    /// <param name="cachedFolder"></param>
    /// <param name="log"></param>
    public CachedEncoder(IEncoder encoder, string cachedFolder, ILogger? log = null)
    {
        this.log = log;
        this.encoder = encoder;
        this.cachedFolder = cachedFolder;
    }

    /// <inheritdoc/>
    public string EncodedExt => this.encoder.EncodedExt;

    /// <inheritdoc/>
    public string[] InputFormats => this.encoder.InputFormats;

    /// <inheritdoc/>
    public async Task Encode(string inputFile, string outputFile, Loop? loop = null)
    {
        var inputChecksum = Checksum.Compute(inputFile);

        var cachedFile = Path.Join(this.cachedFolder, $"{inputChecksum}{this.encoder.EncodedExt}");
        var cachedLoop = loop ?? new();
        var cachedLoopFile = Path.ChangeExtension(cachedFile, ".json");

        if (!File.Exists(cachedFile) || this.LoopChanged(cachedLoopFile, cachedLoop))
        {
            this.log?.LogDebug("Cached file missing or loop mismatch.");
            await this.encoder.Encode(inputFile, cachedFile, cachedLoop);
            SaveLoop(cachedLoopFile, cachedLoop);
        }

        // Copy cached file to output.
        File.Copy(cachedFile, outputFile, true);
    }

    private static void SaveLoop(string loopFile, Loop loop)
    {
        JsonFileSerializer.Serialize(loopFile, loop);
    }

    private bool LoopChanged(string loopFile, Loop loop)
    {
        if (!File.Exists(loopFile))
        {
            return true;
        }

        var cachedLoop = JsonFileSerializer.Deserialize<Loop>(loopFile);
        if (cachedLoop == null)
        {
            this.log?.LogWarning("Failed to deserialize cached file loop.");
            return true;
        }

        return cachedLoop.StartSample != loop.StartSample
            || cachedLoop.EndSample != loop.EndSample
            || cachedLoop.Enabled != loop.Enabled;
    }
}
