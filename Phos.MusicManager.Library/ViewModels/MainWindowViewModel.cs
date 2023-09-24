namespace Phos.MusicManager.Desktop.Library.ViewModels;

using System;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Commands;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels;
using Phos.MusicManager.Library.ViewModels.Music;
using Phos.MusicManager.Library.ViewModels.Projects;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class MainWindowViewModel : WindowViewModelBase
{
    private readonly ProjectsNavigation navigation;
    private readonly ProjectCommands projectCommands;
    private readonly LoopService loopService;
    private readonly ProjectExporter projectExporter;
    private readonly IDialogService dialog;
    private readonly ILogger? log;

    public MainWindowViewModel(
        ViewModelBase rootViewModel,
        NotificationsViewModel notifications,
        ProjectsNavigation navigation,
        ProjectCommands projectCommands,
        LoopService loopService,
        ProjectExporter projectExporter,
        RefreshFilesCommand refreshFilesCommand,
        ClearCacheCommand clearCacheCommand,
        AddFileCommand addFileCommand,
        IDialogService dialog,
        ILogger? log = null)
    {
        this.RootViewModel = rootViewModel;
        this.Notifications = notifications;
        this.navigation = navigation;
        this.loopService = loopService;
        this.projectExporter = projectExporter;
        this.dialog = dialog;
        this.projectCommands = projectCommands;
        this.log = log;

        this.RefreshFilesCommand = refreshFilesCommand;
        this.ClearCacheCommand = clearCacheCommand.Create();
        this.AddEncoderCommand = addFileCommand.AddEncoderCommand;
        this.AddProjectPresetCommand = addFileCommand.AddProjectPresetCommand;

        this.navigation.PropertyChanged += this.Navigation_PropertyChanged;
    }

    public ViewModelBase RootViewModel { get; set; }

    public NotificationsViewModel Notifications { get; }

    public IRelayCommand NewProjectCommand => this.projectCommands.NewProjectCommand;

    public IRelayCommand OpenProjectCommand => this.projectCommands.OpenProjectCommand;

    public IRelayCommand EncodersFolderCommand { get; } = OpenPathCommand.Create(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "audio", "encoders"));

    public IRelayCommand AudioFolderCommand { get; } = OpenPathCommand.Create(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "audio"));

    public IRelayCommand PresetsFolderCommand { get; } = OpenPathCommand.Create(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "presets"));

    public IRelayCommand ClearCacheCommand { get; }

    public ICommand RefreshFilesCommand { get; }

    public ICommand AddEncoderCommand { get; }

    public ICommand AddProjectPresetCommand { get; }

    public bool CanProjectCommand => this.navigation.Current is ProjectViewModel;

    [RelayCommand]
    public override void Close(object? result = null)
    {
        base.Close(result);
    }

    [RelayCommand]
    private async Task ExportPortableProject()
    {
        var outputFile = await this.dialog.OpenSaveFile("Export Portable Project", "Portable Project|*.phos");
        if (outputFile == null)
        {
            return;
        }

        if (this.navigation.Current is ProjectViewModel projectPage)
        {
            try
            {
                this.projectExporter.ExportPortableProject(projectPage.Project, outputFile);
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to export portable project.");
            }
        }
    }

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
                this.projectExporter.ExportProjectPreset(projectPage.Project, presetFile);
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to export project preset.");
            }
        }
    }

    [RelayCommand]
    private async Task OpenQuickLoop()
    {
        var folderDir = await this.dialog.OpenFolderSelect("Select Folder with Audio Files");
        if (folderDir == null)
        {
            return;
        }

        var quickLoop = new QuickLoopViewModel(folderDir, this.loopService);
        await this.dialog.OpenDialog(quickLoop);
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