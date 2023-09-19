namespace Phos.MusicManager.Library.Commands;

using System;
using System.Windows.Input;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Projects;

#pragma warning disable SA1600 // Elements should be documented
public class RefreshFilesCommand : ICommand
{
    private readonly AudioEncoderRegistry encoderRegistry;
    private readonly ProjectPresetRepository presetRepository;

    public RefreshFilesCommand(AudioEncoderRegistry encoderRegistry, ProjectPresetRepository presetRepository)
    {
        this.encoderRegistry = encoderRegistry;
        this.presetRepository = presetRepository;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        this.encoderRegistry.LoadEncoders();
        this.presetRepository.LoadPresets();
    }
}
