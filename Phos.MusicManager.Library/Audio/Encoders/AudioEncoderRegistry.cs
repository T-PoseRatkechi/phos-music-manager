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
        var parser = new FileIniDataParser();
        foreach (var file in Directory.EnumerateFiles(this.encodersDir, "*.ini", SearchOption.AllDirectories))
        {
            try
            {
                var data = parser.ReadFile(file);
                var config = new Config
                {
                    Name = data.GetKey("name") ?? throw new Exception("Name is missing."),
                    OutContainerFormat = data.GetKey("out_container_format")?.ToUpper() ?? throw new Exception("Missing output container format."),
                    OutFormat = data.GetKey("out_format")?.ToUpper(),
                    LoopAlignment = int.TryParse(data.GetKey("loop_alignment"), out var loopAlignment) ? loopAlignment : default,
                    BlockSize = int.TryParse(data.GetKey("block_size"), out var blockSize) ? blockSize : default,
                    Version = int.TryParse(data.GetKey("version"), out var version) ? version : default,
                    FrameSize = int.TryParse(data.GetKey("frame_size"), out var frameSize) ? frameSize : default,
                    Filter = int.TryParse(data.GetKey("filter"), out var filter) ? filter : 2,
                    AdxType = Enum.TryParse<CriAdxType>(data.GetKey("adx_type"), out var adxType) ? adxType : default,
                    KeyString = data.GetKey("key_string"),
                    KeyCode = ulong.TryParse(data.GetKey("key_code"), out var keyCode) ? keyCode : default,
                    Endianness = Enum.TryParse<Endianness>(data.GetKey("endianess"), out var endianess) ? endianess : default,
                    HcaQuality = Enum.TryParse<CriHcaQuality>(data.GetKey("hca_quality"), out var hcaQuality) ? hcaQuality : default,
                    Bitrate = int.TryParse(data.GetKey("bitrate"), out var bitrate) ? bitrate : default,
                    LimitBitrate = bool.TryParse(data.GetKey("limit_bitrate"), out var limitBitrate) && limitBitrate,
                    EncodeCbr = bool.TryParse(data.GetKey("encode_cbr"), out var encodeCbr) && encodeCbr,
                };

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
