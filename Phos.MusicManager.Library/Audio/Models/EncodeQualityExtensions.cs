namespace Phos.MusicManager.Library.Audio.Models;

using VGAudio.Codecs.CriHca;

#pragma warning disable SA1600 // Elements should be documented
public static class EncodeQualityExtensions
{
    public static CriHcaQuality ToHcaQuality(this EncodeQuality quality)
    {
        return quality switch
        {
            EncodeQuality.Highest => CriHcaQuality.Highest,
            EncodeQuality.High => CriHcaQuality.High,
            EncodeQuality.Medium => CriHcaQuality.Middle,
            EncodeQuality.Low => CriHcaQuality.Low,
            EncodeQuality.Lowest => CriHcaQuality.Lowest,
            _ => CriHcaQuality.NotSet,
        };
    }
}
