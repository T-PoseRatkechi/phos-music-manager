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
    private readonly LoopService loopService;
    private readonly IDialogService dialog;

    public MusicFactory(AudioEncoderRegistry encoderRegistry, LoopService loopService, IDialogService dialog)
    {
        this.loopService = loopService;
        this.dialog = dialog;
        this.encoders = encoderRegistry.Encoders.Keys.ToArray();
    }

    public TrackPanelViewModel CreateTrackPanel(AudioTrack track, AudioService audioManager, ICommand closeCommand)
    {
        return new(track, audioManager, this.loopService, this.encoders, this.dialog, closeCommand);
    }

    public AddTrackViewModel CreateAddTrack()
    {
        return new(this.encoders);
    }
}
