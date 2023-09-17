namespace Phos.MusicManager.Library.ViewModels.Projects.Forms;

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Phos.MusicManager.Library.Projects;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class CreateProjectForm : ObservableValidator
{
    public const string NoneOption = "None";
    private readonly ProjectPresetRepository presetRepo;
    private readonly ProjectRepository projectRepo;
    private readonly Project? existingProject;

    private string name = string.Empty;
    private string selectedPreset = NoneOption;

    [ObservableProperty]
    private string color = string.Empty;

    [ObservableProperty]
    private string outputDir = string.Empty;

    [ObservableProperty]
    private string gameInstallPath = string.Empty;

    public CreateProjectForm(
        ProjectRepository projectRepo,
        ProjectPresetRepository presetRepo,
        Project? existingProject = null)
    {
        this.presetRepo = presetRepo;
        this.projectRepo = projectRepo;
        this.existingProject = existingProject;

        this.PresetOptions = presetRepo.List.Select(x => x.Name).ToList();
        this.PresetOptions.Insert(0, NoneOption);
        this.PostBuildOptions = new();
        this.PostBuildOptions.Insert(0, NoneOption);

        if (existingProject != null)
        {
            this.Name = existingProject.Settings.Value.Name;
            this.Color = existingProject.Settings.Value.Color ?? string.Empty;
            this.SelectedPreset = existingProject.Settings.Value.Preset ?? NoneOption;
            this.SelectedPostBuild = existingProject.Settings.Value.PostBuild ?? NoneOption;
            this.GameInstallPath = existingProject.Settings.Value.GameInstallPath ?? string.Empty;
            this.OutputDir = existingProject.Settings.Value.OutputDir ?? string.Empty;
        }
        else
        {
            this.SelectedPostBuild = NoneOption;
        }

        this.ValidateAllProperties();
    }

    [CustomValidation(typeof(CreateProjectForm), nameof(ValidateName))]
    public string Name
    {
        get => this.name;
        set => this.SetProperty(ref this.name, value, true);
    }

    public string SelectedPreset
    {
        get => this.selectedPreset;
        set
        {
            this.SetProperty(ref this.selectedPreset, value);
            this.SetPresetValues();
        }
    }

    public List<string> PresetOptions { get; }

    public string SelectedPostBuild { get; set; }

    public List<string> PostBuildOptions { get; }

    public static ValidationResult? ValidateName(string name, ValidationContext context)
    {
        var instance = (CreateProjectForm)context.ObjectInstance;

        // Ignore duplicate name if editing project.
        if (instance.existingProject != null && instance.Name == instance.existingProject.Settings.Value.Name)
        {
            return ValidationResult.Success;
        }

        if (string.IsNullOrEmpty(name))
        {
            return new(null);
        }

        if (instance.projectRepo.List.FirstOrDefault(x => x.Settings.Value.Name == name) != null)
        {
            return new("Project with name already exists.");
        }

        return ValidationResult.Success;
    }

    private void SetPresetValues()
    {
        if (this.selectedPreset == NoneOption)
        {
            return;
        }

        var preset = this.presetRepo.GetById(this.selectedPreset);
        if (preset != null)
        {
            // Only set color from preset if new project.
            if (this.existingProject == null)
            {
                this.Color = preset.Color ?? string.Empty;
            }

            this.SelectedPostBuild = preset.PostBuild ?? string.Empty;
        }
    }
}
