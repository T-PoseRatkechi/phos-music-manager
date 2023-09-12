namespace Phos.MusicManager.Library.ViewModels.Music;

using System.Windows.Input;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;

#pragma warning disable SA1600 // Elements should be documented
public class TrackPanelFactory
{
    private readonly string[] encoders;
    private readonly IDialogService dialog;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackPanelFactory"/> class.
    /// </summary>
    /// <param name="encoderRegistry"></param>
    /// <param name="dialog"></param>
    public TrackPanelFactory(AudioEncoderRegistry encoderRegistry, IDialogService dialog)
    {
        this.encoders = encoderRegistry.Encoders.Keys.ToArray();
        this.dialog = dialog;
    }

    public TrackPanelViewModel Create(AudioTrack track, AudioService audioManager, ICommand closeCommand)
    {
        return new(track, audioManager, this.encoders, this.dialog, closeCommand);
    }
}
