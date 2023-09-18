namespace Phos.MusicManager.Library.Audio.Encoders.VgAudio;

using VGAudio.Containers;

#pragma warning disable SA1600 // Elements should be documented
internal class ContainerType
{
    public ContainerType(IEnumerable<string> names, Func<IAudioReader> getReader, Func<IAudioWriter> getWriter, Func<Config, Configuration> getConfiguration)
    {
        this.Names = names;
        this.GetReader = getReader;
        this.GetWriter = getWriter;
        this.GetConfiguration = getConfiguration;
    }

    public IEnumerable<string> Names { get; }

    public Func<IAudioReader> GetReader { get; }

    public Func<IAudioWriter> GetWriter { get; }

    public Func<Config, Configuration> GetConfiguration { get; }
}
