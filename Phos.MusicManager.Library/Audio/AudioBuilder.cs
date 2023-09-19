namespace Phos.MusicManager.Library.Audio;

using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Audio.Models;

/// <summary>
/// Audio builder.
/// </summary>
public class AudioBuilder
{
    private readonly ILogger? log;
    private readonly AudioEncoderRegistry encoderRegistry;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioBuilder"/> class.
    /// </summary>
    /// <param name="encoderRegistry"></param>
    /// <param name="log"></param>
    public AudioBuilder(AudioEncoderRegistry encoderRegistry, ILogger? log = null)
    {
        this.log = log;
        this.encoderRegistry = encoderRegistry;
    }

    /// <summary>
    /// Builds the audio tracks.
    /// </summary>
    /// <param name="tracks">Tracks to build.</param>
    /// <param name="outputDir">Path to output build.</param>
    /// <param name="progress">Progress reporter.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Build(IEnumerable<AudioTrack> tracks, string outputDir, IProgress<int>? progress = null)
    {
        var numTracksBuilt = 0;
        foreach (var track in tracks)
        {
            if (string.IsNullOrEmpty(track.OutputPath))
            {
                throw new ArgumentException($"Track output path is missing.\nTrack: {track.Name}");
            }

            var outputFile = Path.Join(outputDir, track.OutputPath);

            // Delete previous output file.
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            if (track.ReplacementFile == null)
            {
                numTracksBuilt++;
                progress?.Report(numTracksBuilt);
                continue;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);
            if (this.encoderRegistry.Encoders.TryGetValue(track.Encoder, out var encoder))
            {
                await encoder.Encode(track.ReplacementFile, outputFile, track.Loop);
            }
            else
            {
                this.log?.LogWarning("Unknown encoder {encoder} for track {track}.", track.Encoder, track.Name);
            }

            numTracksBuilt++;
            progress?.Report(numTracksBuilt);
        }
    }
}
