namespace Phos.MusicManager.Library.Audio.Encoders.VgAudio;

using Phos.MusicManager.Library.Audio.Encoders.Exceptions;
using Phos.MusicManager.Library.Audio.Models;
using VGAudio.Containers;

#pragma warning disable SA1600 // Elements should be documented
public class VgAudioEncoder : IEncoder
{
    private readonly ContainerType outputContainer;
    private readonly IAudioWriter writer;
    private readonly Configuration? configuration;

    public VgAudioEncoder(Config config)
    {
        if (config.OutContainerFormat == null)
        {
            throw new ArgumentException("Config missing output container format.");
        }

        this.outputContainer = ContainerTypes.Containers.First(container => container.Value.Names.Contains(config.OutContainerFormat.ToLower())).Value;
        this.writer = this.outputContainer.GetWriter();
        this.configuration = this.outputContainer.GetConfiguration(config);
        this.EncodedExt = $".{config.OutContainerFormat.ToLower()}";
    }

    public string EncodedExt { get; }

    public string[] InputTypes { get; } = ContainerTypes.ExtensionList.Select(x => $".{x}").ToArray();

    public Task Encode(string inputFile, string outputFile, Loop? loop = null)
    {
        return Task.Run(() =>
        {
            // If same file type, just copy to output.
            var inputFileExt = Path.GetExtension(inputFile).Trim('.');

            using var inputStream = File.OpenRead(inputFile);
            using var outputStream = File.Create(outputFile);

            var readerContainer = ContainerTypes.Containers.First(x => x.Value.Names.Contains(inputFileExt, StringComparer.OrdinalIgnoreCase)).Value;
            var reader = readerContainer.GetReader();
            var inputAudio = reader.Read(inputStream);

            try
            {
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
            }
            catch (Exception)
            {
            }

            this.writer.WriteToStream(inputAudio, outputStream, this.configuration);
        });
    }
}
