namespace Phos.MusicManager.Library.Audio.Encoders;

using IniParser;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Encoders.VgAudio;
using VGAudio.Codecs.CriAdx;
using VGAudio.Codecs.CriHca;
using VGAudio.Utilities;

#pragma warning disable SA1600 // Elements should be documented
public class AudioEncoderRegistry
{
    private readonly string encodersDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "audio", "encoders");
    private readonly string cacheDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "audio", "cached");
    private readonly ILogger? log;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioEncoderRegistry"/> class.
    /// </summary>
    /// <param name="log"></param>
    public AudioEncoderRegistry(ILogger? log)
    {
        this.log = log;
        Directory.CreateDirectory(this.encodersDir);
        Directory.CreateDirectory(this.cacheDir);

        this.LoadEncoders();
    }

    public Dictionary<string, IEncoder> Encoders { get; private set; } = new();

    public Dictionary<string, string> EncoderFiles { get; private set; } = new();

    public string EncodersFolder => this.encodersDir;

    public void Add(string encoderFile)
    {
        var encoderFileName = Path.GetFileName(encoderFile);
        File.Copy(encoderFile, Path.Join(this.encodersDir, encoderFileName), true);
        this.LoadEncoders();
    }

    public void LoadEncoders()
    {
        this.Encoders.Clear();
        this.EncoderFiles.Clear();
        this.LoadVgAudioEncoders();
    }

    private void LoadVgAudioEncoders()
    {
        // Add default encoders.
        foreach (var container in ContainerTypes.Containers)
        {
            try
            {
                var type = container.Value;
                var name = type.Names.First();
                var encoder = new VgAudioEncoder(new() { OutContainerFormat = name });
                this.Encoders.Add(name.ToUpper(), this.CreateCachedEncoder(encoder, name));
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to create default encoder.");
            }
        }

        // Add from ini configs.
        foreach (var file in Directory.EnumerateFiles(this.encodersDir, "*.ini", SearchOption.AllDirectories))
        {
            try
            {
                var config = ConfigParser.Parse(file);
                var encoder = new VgAudioEncoder(config);
                var cachedName = Path.GetFileNameWithoutExtension(file);
                this.Encoders.Add(config.Name, this.CreateCachedEncoder(encoder, cachedName));
                this.EncoderFiles.Add(config.Name, file);
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to create encoder from file.\nFile: {file}", file);
            }
        }
    }

    private CachedEncoder CreateCachedEncoder(IEncoder encoder, string cacheName)
    {
        var encoderCacheDir = Directory.CreateDirectory(Path.Join(this.cacheDir, cacheName));
        encoderCacheDir.Create();

        return new CachedEncoder(encoder, encoderCacheDir.FullName, this.log);
    }
}
