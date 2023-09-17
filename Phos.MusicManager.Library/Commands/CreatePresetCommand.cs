namespace Phos.MusicManager.Library.Commands;

using System;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Projects;

#pragma warning disable SA1600 // Elements should be documented
public class CreatePresetCommand
{
    private readonly ILogger? log;
    private readonly ProjectPresetRepository presetRepository;

    public CreatePresetCommand(ProjectPresetRepository presetRepository, ILogger? log = null)
    {
        this.log = log;
        this.presetRepository = presetRepository;
    }

    public IRelayCommand Create(Project project, Func<bool>? canExecute = null) =>
        canExecute == null ? new RelayCommand(() => this.CreatePreset(project)) : new RelayCommand(() => this.CreatePreset(project), canExecute);

    private void CreatePreset(Project project)
    {
        try
        {
            this.presetRepository.Create(project);
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to create preset from project {project}.", project.Settings.Value.Name);
        }
    }
}
