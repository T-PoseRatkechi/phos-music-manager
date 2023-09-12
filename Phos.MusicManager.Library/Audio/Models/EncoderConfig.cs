namespace Phos.MusicManager.Library.Audio.Models;

/// <summary>
/// Encoder config.
/// </summary>
public class EncoderConfig
{
    /// <summary>
    /// Gets or sets the encoder name.
    /// </summary>
    public string Name { get; set; } = "Unknown";

    /// <summary>
    /// Gets or sets the encoder to use.
    /// </summary>
    public string Encoder { get; set; } = "HCA";

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
