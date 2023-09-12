namespace Phos.MusicManager.Library.Audio.Encoders;

using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Encoders.Exceptions;
using Phos.MusicManager.Library.Audio.Models;
using VGAudio.Containers.Adx;
using VGAudio.Containers.Wave;

/// <summary>
/// ADX encoder.
/// </summary>
public class AdxEncoder : IEncoder
{
    private readonly ILogger? log;
    private readonly AdxWriter writer = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AdxEncoder"/> class.
    /// </summary>
    /// <param name="log"></param>
    public AdxEncoder(ILogger? log = null)
    {
        this.log = log;
    }

    /// <inheritdoc/>
    public string EncodedExt { get; } = ".adx";

    /// <inheritdoc/>
    public string[] InputFormats { get; } = new string[] { ".wav" };

    /// <inheritdoc/>
    public Task Encode(string inputFile, string outputFile, Loop? loop = null)
    {
        return Task.Run(() =>
        {
            using var inputStream = File.OpenRead(inputFile);
            using var outputStream = File.Create(outputFile);

            var inputAudio = new WaveReader().Read(inputStream);
            if (loop == null)
            {
                inputAudio.SetLoop(false);
            }
            else
            {
                if (loop.StartSample == 0 && loop.EndSample == 0)
                {
                    inputAudio.SetLoop(loop.Enabled);
                }
                else if (loop.StartSample >= loop.EndSample)
                {
                    throw new InvalidLoopException(loop);
                }
                else
                {
                    inputAudio.SetLoop(loop.Enabled, loop.StartSample, loop.EndSample);
                }
            }

            // ADX writer seems to be multi-threaded, causing issues with UI logger.
            //this.writer.Configuration.Progress = new LoggerProgressReport(this.log);
            this.writer.WriteToStream(inputAudio, outputStream);
        });
    }
}
