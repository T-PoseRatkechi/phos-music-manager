namespace Phos.MusicManager.Library.ViewModels.Music;

using System.Windows.Input;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.ViewModels.Music.Dialogs;

#pragma warning disable SA1600 // Elements should be documented
public class MusicFactory
{
    private readonly string[] encoders;
    private readonly IDialogService dialog;

    /// <summary>
    /// Initializes a new instance of the <see cref="MusicFactory"/> class.
    /// </summary>
    /// <param name="encoderRegistry"></param>
    /// <param name="dialog"></param>
    public MusicFactory(AudioEncoderRegistry encoderRegistry, IDialogService dialog)
    {
        this.encoders = encoderRegistry.Encoders.Keys.ToArray();
        this.dialog = dialog;
    }

    public TrackPanelViewModel CreateTrackPanel(AudioTrack track, AudioService audioManager, ICommand closeCommand)
    {
        return new(track, audioManager, this.encoders, this.dialog, closeCommand);
    }

    public AddTrackViewModel CreateAddTrack()
    {
        return new(this.encoders);
    }

}
