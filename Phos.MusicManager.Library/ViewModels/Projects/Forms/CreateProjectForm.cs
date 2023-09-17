namespace Phos.MusicManager.Library.ViewModels.Projects.Forms;

using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Phos.MusicManager.Library.Projects;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class CreateProjectForm : ObservableValidator, IDisposable
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

    [ObservableProperty]
    private string[] presetOptions;

    public CreateProjectForm(
        ProjectRepository projectRepo,
        ProjectPresetRepository presetRepo,
        Project? existingProject = null)
    {
        this.presetRepo = presetRepo;
        this.projectRepo = projectRepo;
        this.existingProject = existingProject;

        this.presetOptions = new string[] { NoneOption }.Concat(presetRepo.List.Select(x => x.Name)).ToArray();
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

            this.presetRepo.List.CollectionChanged += this.List_CollectionChanged;
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

    public string SelectedPostBuild { get; set; }

    public List<string> PostBuildOptions { get; }

    public static ValidationResult? ValidateName(string name, ValidationContext context)
    {
        if (string.IsNullOrEmpty(name))
        {
            return new(null);
        }

        return ValidationResult.Success;
    }

    public void Dispose()
    {
        this.presetRepo.List.CollectionChanged -= this.List_CollectionChanged;
        GC.SuppressFinalize(this);
    }

    private void List_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Sync preset options with presets list.
        this.PresetOptions = new string[] { NoneOption }.Concat(this.presetRepo.List.Select(x => x.Name)).ToArray();
        if (e.NewItems != null)
        {
            // New preset was added from project, set as current preset.
            var newPresetFromProject = e.NewItems.Cast<ProjectPreset>().FirstOrDefault(x => x.Name == this.existingProject?.Settings.Value.Name);
            if (newPresetFromProject != null)
            {
                this.SelectedPreset = newPresetFromProject.Name;
            }
        }
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
