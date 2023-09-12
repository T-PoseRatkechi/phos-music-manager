namespace Phos.MusicManager.Library.Audio.Processors;

/// <summary>
/// Processor config.
/// </summary>
public class ProcessorConfig
{
    /// <summary>
    /// Gets or sets the processor name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the encoder to use.
    /// </summary>
    public string Encoder { get; set; } = Processors.Encoder.HCA;

    /// <summary>
    /// Gets or sets the encode quality.
    /// </summary>
    public EncodeQuality Quality { get; set; } = EncodeQuality.High;

    /// <summary>
    /// Gets or sets the bitrate.
    /// </summary>
    public int Bitrate { get; set; }

    /// <summary>
    /// Gets or sets the encryption key.
    /// </summary>
    public ulong EncryptionKey { get; set; }

    /// <summary>
    /// Gets or sets the encryption type.
    /// </summary>
    public int EncryptionType { get; set; }
}
