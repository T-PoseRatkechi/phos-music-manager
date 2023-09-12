namespace Phos.MusicManager.Library.Audio.Encoders;

using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Encoders.Exceptions;
using Phos.MusicManager.Library.Audio.Models;
using VGAudio.Containers.Hca;
using VGAudio.Containers.Wave;

/// <summary>
/// HCA encoder.
/// </summary>
public class HcaEncoder : IEncoder
{
    private readonly ILogger? log;
    private readonly HcaWriter writer = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="HcaEncoder"/> class.
    /// </summary>
    /// <param name="log"></param>
    public HcaEncoder(ILogger? log = null)
    {
        this.log = log;
    }

    /// <inheritdoc/>
    public string EncodedExt { get; } = ".hca";

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
            if (loop == null || !loop.Enabled)
            {
                inputAudio.SetLoop(false);
            }
            else
            {
                if (loop.Enabled && loop.StartSample == 0 && loop.EndSample == 0)
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
