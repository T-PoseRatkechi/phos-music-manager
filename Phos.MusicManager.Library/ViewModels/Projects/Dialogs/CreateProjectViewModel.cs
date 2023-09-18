namespace Phos.MusicManager.Library.ViewModels.Projects.Dialogs;

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Commands;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels.Projects.Forms;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class CreateProjectViewModel : WindowViewModelBase
{
    private readonly Project? existingProject;
    private readonly ProjectPresetRepository presetRepo;
    private readonly IDialogService dialog;

    /// <summary>
    /// Icon byte array or file path.
    /// </summary>
    [ObservableProperty]
    private object? icon;

    public CreateProjectViewModel(
        ProjectRepository projectRepo,
        ProjectPresetRepository presetRepo,
        CreatePresetCommand createPresetCommand,
        IDialogService dialog,
        Project? existingProject = null)
    {
        this.dialog = dialog;
        this.presetRepo = presetRepo;
        this.Form = new(projectRepo, presetRepo, existingProject);
        if (existingProject != null)
        {
            this.existingProject = existingProject;
            this.IsEditing = true;
            this.Icon = Path.Join(existingProject.ProjectFolder, "icon.png");
            this.CreatePresetCommand = createPresetCommand.Create(existingProject);
        }

        this.Form.ErrorsChanged += this.Form_ErrorsChanged;
        this.Form.PropertyChanged += this.Form_PropertyChanged;
    }

    public string? Id => this.existingProject?.Settings.Value.Id;

    public bool IsEditing { get; init; }

    public CreateProjectForm Form { get; }

    public IRelayCommand? CreatePresetCommand { get; }

    private bool CanConfirm => !this.Form.HasErrors;

    [RelayCommand]
    public override void Close(object? result = null)
    {
        this.Form.ErrorsChanged -= this.Form_ErrorsChanged;
        this.Form.PropertyChanged -= this.Form_PropertyChanged;
        this.Form.Dispose();
        base.Close(result);
    }

    [RelayCommand(CanExecute = nameof(this.CanConfirm))]
    private void Confirm()
    {
        var projectSettings = new ProjectSettings
        {
            Name = this.Form.Name,
            Preset = this.Form.SelectedPreset != CreateProjectForm.NoneOption ? this.Form.SelectedPreset : null,
            Color = string.IsNullOrEmpty(this.Form.Color) ? null : this.Form.Color,
            GameInstallPath = string.IsNullOrEmpty(this.Form.GameInstallPath) ? null : this.Form.GameInstallPath,
            OutputDir = string.IsNullOrEmpty(this.Form.OutputDir) ? null : this.Form.OutputDir,
            PostBuild = string.IsNullOrEmpty(this.Form.SelectedPostBuild) ? null : this.Form.SelectedPostBuild,
        };

        this.Close(projectSettings);
    }

    [RelayCommand]
    private async Task SelectOutputFolder()
    {
        var outputDir = await this.dialog.OpenFolderSelect("Select Output Folder");
        if (outputDir != null)
        {
            this.Form.OutputDir = outputDir;
        }
    }

    [RelayCommand]
    private async Task SelectGameInstall()
    {
        var gameInstallPath = await this.dialog.OpenFileSelect("Select Game Install");
        if (gameInstallPath != null)
        {
            this.Form.GameInstallPath = gameInstallPath;
        }
    }

    [RelayCommand]
    private async Task SelectIcon()
    {
        var iconFile = await this.dialog.OpenFileSelect("Select Project Icon", "PNG files|*.png");
        if (iconFile != null)
        {
            this.Icon = iconFile;
        }
    }

    [RelayCommand]
    private void RemoveIcon()
    {
        this.Icon = null;
    }

    [RelayCommand]
    private void ResetProject()
    {
        if (this.existingProject == null)
        {
            return;
        }

        // Use form's selected preset instead of project's.
        // It's either the same or if different probably the intended default.
        if (this.presetRepo.GetById(this.Form.SelectedPreset) is ProjectPreset preset)
        {
            this.existingProject.Audio.Tracks.Clear();
            this.existingProject.Audio.AddTracks(preset.DefaultTracks.Select(x => new AudioTrack
            {
                Name = x.Name,
                Category = x.Category,
                Tags = x.Tags,
                OutputPath = x.OutputPath,
                Encoder = x.Encoder,
            }));
        }

        this.existingProject.Settings.Value.OutputDir = null;
        this.existingProject.Settings.Value.GameInstallPath = null;
        this.existingProject.Settings.Save();
    }

    private void Form_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Preset changed.
        if (e.PropertyName == nameof(this.Form.SelectedPreset))
        {
            // Display preset icon if creating new project and no icon file has been set yet.
            if (!this.IsEditing &&
                this.Form.SelectedPreset != CreateProjectForm.NoneOption &&
                this.Icon is not string)
            {
                var currentPreset = this.presetRepo.GetById(this.Form.SelectedPreset);
                if (currentPreset == null)
                {
                    return;
                }

                this.Icon = currentPreset.Icon;
            }
        }
    }

    private void Form_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        this.ConfirmCommand.NotifyCanExecuteChanged();
    }
}
