namespace Phos.MusicManager.Desktop.Library.ViewModels;

using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Commands;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels;
using Phos.MusicManager.Library.ViewModels.Projects;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ProjectsNavigation navigation;
    private readonly ProjectPresetRepository presetRepo;
    private readonly IDialogService dialog;
    private readonly ILogger? log;
    private readonly ProjectCommands projectCommands;

    public MainWindowViewModel(
        ViewModelBase rootViewModel,
        ProjectsNavigation navigation,
        ProjectPresetRepository presetRepo,
        ProjectCommands projectCommands,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.RootViewModel = rootViewModel;
        this.navigation = navigation;
        this.presetRepo = presetRepo;
        this.dialog = dialog;
        this.projectCommands = projectCommands;
        this.log = log;

        this.NewProjectCommand = this.projectCommands.Create_NewProjectCommand();
        this.navigation.PropertyChanged += this.Navigation_PropertyChanged;
    }

    public ViewModelBase RootViewModel { get; set; }

    public IRelayCommand NewProjectCommand { get; }

    public bool CanProjectCommand => this.navigation.Current is ProjectViewModel;

    [RelayCommand(CanExecute = nameof(this.CanProjectCommand))]
    private async Task ExportPreset()
    {
        var presetFile = await this.dialog.OpenSaveFile("Export Project Preset", "Project Preset|*.project");
        if (presetFile == null)
        {
            return;
        }

        if (this.navigation.Current is ProjectViewModel projectPage)
        {
            try
            {
                this.presetRepo.Create(projectPage.Project, presetFile);
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to export project preset.");
            }
        }
    }

    private void Navigation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(this.navigation.Current))
        {
            this.OnPropertyChanged(nameof(this.CanProjectCommand));
            this.ExportPresetCommand.NotifyCanExecuteChanged();
        }
    }
}