﻿namespace Phos.MusicManager.Library.Audio.Encoders.VgAudio;

using VGAudio.Codecs.CriAdx;
using VGAudio.Codecs.CriHca;
using VGAudio.Containers;
using VGAudio.Containers.Adx;
using VGAudio.Containers.Dsp;
using VGAudio.Containers.Hca;
using VGAudio.Containers.Hps;
using VGAudio.Containers.Idsp;
using VGAudio.Containers.NintendoWare;
using VGAudio.Containers.Wave;

#pragma warning disable SA1600 // Elements should be documented
internal static class CreateConfiguration
{
    public static Configuration Wave(Config config)
    {
        var waveConfig = new WaveConfiguration();

        switch (config.OutFormat)
        {
            case AudioFormat.GC_ADPCM:
                throw new InvalidDataException("Can't use format GcAdpcm with Wave files");
            case AudioFormat.PCM16:
                waveConfig.Codec = WaveCodec.Pcm16Bit;
                break;
            case AudioFormat.PCM8:
                waveConfig.Codec = WaveCodec.Pcm8Bit;
                break;
        }

        return waveConfig;
    }

    public static Configuration Dsp(Config config)
    {
        var dspConfig = new DspConfiguration();

        switch (config.OutFormat)
        {
            case AudioFormat.PCM16:
                throw new InvalidDataException("Can't use format PCM16 with DSP files");
            case AudioFormat.PCM8:
                throw new InvalidDataException("Can't use format PCM8 with DSP files");
        }

        if (config.LoopAlignment > 0)
        {
            dspConfig.LoopPointAlignment = config.LoopAlignment;
        }

        return dspConfig;
    }

    public static Configuration Idsp(Config config)
    {
        var idspConfig = new IdspConfiguration();

        switch (config.OutFormat)
        {
            case AudioFormat.PCM16:
                throw new InvalidDataException("Can't use format PCM16 with IDSP files");
            case AudioFormat.PCM8:
                throw new InvalidDataException("Can't use format PCM8 with IDSP files");
        }

        if (config.BlockSize > 0)
        {
            idspConfig.BlockSize = config.BlockSize;
        }

        return idspConfig;
    }

    public static Configuration Bxstm(Config config)
    {
        var bxstmConfig = new BxstmConfiguration
        {
            Endianness = config.Endianness,
        };

        switch (config.OutFormat)
        {
            case AudioFormat.GC_ADPCM:
                bxstmConfig.Codec = NwCodec.GcAdpcm;
                break;
            case AudioFormat.PCM16:
                bxstmConfig.Codec = NwCodec.Pcm16Bit;
                break;
            case AudioFormat.PCM8:
                bxstmConfig.Codec = NwCodec.Pcm8Bit;
                break;
        }

        if (config.LoopAlignment > 0)
        {
            bxstmConfig.LoopPointAlignment = config.LoopAlignment;
        }

        return bxstmConfig;
    }

    public static Configuration Hps(Config config)
    {
        var hpsConfig = new HpsConfiguration();

        switch (config.OutFormat)
        {
            case AudioFormat.PCM16:
                throw new InvalidDataException("Can't use format PCM16 with HPS files");
            case AudioFormat.PCM8:
                throw new InvalidDataException("Can't use format PCM8 with HPS files");
            default:
                break;
        }

        return hpsConfig;
    }

    public static Configuration Adx(Config config)
    {
        var adxConfig = new AdxConfiguration();

        if (config.Version != 0)
        {
            adxConfig.Version = config.Version;
        }

        if (config.FrameSize != 0)
        {
            adxConfig.FrameSize = config.FrameSize;
        }

        if (config.Filter >= 0 && config.Filter <= 3)
        {
            adxConfig.Filter = config.Filter;
        }

        if (config.AdxType != default)
        {
            adxConfig.Type = config.AdxType;
        }

        if (config.KeyString != null)
        {
            adxConfig.EncryptionKey = new CriAdxKey(config.KeyString);
            adxConfig.EncryptionType = 8;
        }

        if (config.KeyCode != 0)
        {
            adxConfig.EncryptionKey = new CriAdxKey(config.KeyCode);
            adxConfig.EncryptionType = 9;
        }

        return adxConfig;
    }

    public static Configuration Hca(Config config)
    {
        var hcaConfig = new HcaConfiguration();

        if (config.HcaQuality != CriHcaQuality.NotSet)
        {
            hcaConfig.Quality = config.HcaQuality;
        }

        hcaConfig.LimitBitrate = config.LimitBitrate;
        if (config.Bitrate != 0)
        {
            hcaConfig.Bitrate = config.Bitrate;
        }

        if (config.KeyCode != 0)
        {
            hcaConfig.EncryptionKey = new CriHcaKey(config.KeyCode);
        }

        return hcaConfig;
    }
}
