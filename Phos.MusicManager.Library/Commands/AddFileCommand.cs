namespace Phos.MusicManager.Library.Commands;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class AddFileCommand : ObservableObject
{
    private readonly ILogger? log;
    private readonly AudioEncoderRegistry encoderRegistry;
    private readonly ProjectPresetRepository presetRepository;
    private readonly IDialogService dialog;

    public AddFileCommand(
        AudioEncoderRegistry encoderRegistry,
        ProjectPresetRepository presetRepository,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.log = log;
        this.encoderRegistry = encoderRegistry;
        this.presetRepository = presetRepository;
        this.dialog = dialog;
    }

    [RelayCommand]
    private async Task AddProjectPreset()
    {
        var presetFile = await this.dialog.OpenFileSelect("Select Project Preset", "Project Preset|*.project");
        if (presetFile == null)
        {
            return;
        }

        try
        {
            this.presetRepository.Add(presetFile);
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to add project preset.");
        }
    }

    [RelayCommand]
    private async Task AddEncoder()
    {
        var encoderFile = await this.dialog.OpenFileSelect("Select Encoder Config", "Encoder Config|*.ini");
        if (encoderFile == null)
        {
            return;
        }

        try
        {
            this.encoderRegistry.Add(encoderFile);
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to add encoder config.");
        }
    }
}
