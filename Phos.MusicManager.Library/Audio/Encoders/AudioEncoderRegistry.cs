namespace Phos.MusicManager.Library.Audio.Encoders;

using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common.Serializers;
using VGAudio.Containers.Adx;
using VGAudio.Containers.Hca;

/// <summary>
/// Audio encoder registry.
/// </summary>
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

        this.Encoders = this.GetEncoders();
    }

    /// <summary>
    /// Gets available encoders.
    /// </summary>
    public Dictionary<string, IEncoder> Encoders { get; }

    private Dictionary<string, IEncoder> GetEncoders()
    {
        var encoders = new Dictionary<string, IEncoder>()
        {
            { "HCA", this.CreateCachedEncoder(new HcaEncoder(this.log), "hca") },
            { "ADX", this.CreateCachedEncoder(new AdxEncoder(this.log), "adx") },
        };

        foreach (var configFile in Directory.EnumerateFiles(this.encodersDir, "*.json", SearchOption.AllDirectories))
        {
            try
            {
                var config = JsonFileSerializer.Deserialize<EncoderConfig>(configFile)!;
                if (encoders.ContainsKey(config.Name))
                {
                    this.log?.LogWarning("Duplicate encoder with name {name}.\nFile: {file}", config.Name, configFile);
                    continue;
                }

                var encoder = this.CreateCachedEncoder(this.CreateEncoder(config), Path.GetFileNameWithoutExtension(configFile));
                encoders.Add(config.Name, encoder);
            }
            catch (Exception ex)
            {
                this.log?.LogWarning(ex, "Failed to create encoder with config.\nFile: {file}", configFile);
            }
        }

        return encoders;
    }

    private IEncoder CreateEncoder(EncoderConfig config)
    {
        switch (config.Encoder)
        {
            case "HCA":
                var hcaConfig = new HcaConfiguration
                {
                    Bitrate = config.Bitrate,
                    EncryptionKey = new(config.EncryptionKey),
                    Quality = config.Quality.ToHcaQuality(),
                };

                return new HcaEncoder(hcaConfig, this.log);
            case "ADX":
                var adxConfig = new AdxConfiguration
                {
                    EncryptionKey = new(config.EncryptionKey),
                    EncryptionType = config.EncryptionType,
                };

                return new AdxEncoder(adxConfig, this.log);
            default: throw new ArgumentException($"Unknown encoder: {config.Encoder}");
        }
    }

    private CachedEncoder CreateCachedEncoder(IEncoder encoder, string cacheName)
    {
        var encoderCacheDir = Directory.CreateDirectory(Path.Join(this.cacheDir, cacheName));
        encoderCacheDir.Create();

        return new CachedEncoder(encoder, encoderCacheDir.FullName, this.log);
    }
}
