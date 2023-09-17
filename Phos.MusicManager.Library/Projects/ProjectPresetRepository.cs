namespace Phos.MusicManager.Library.Projects;

using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Serializers;

#pragma warning disable SA1600 // Elements should be documented
public class ProjectPresetRepository
{
    public const string PresetExt = ".project";
    private readonly ILogger? log;
    private readonly string presetsDir;

    public ProjectPresetRepository(ILogger? log = null)
    {
        this.log = log;
        this.presetsDir = Directory.CreateDirectory(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "presets")).FullName;
        this.LoadPresets();
        this.AddDefaultPresets();
    }

    public ObservableCollection<ProjectPreset> List { get; } = new();

    public void Create(ProjectPreset preset, string? outputFile = null)
    {
        var presetOutputFile = outputFile ?? Path.Join(this.presetsDir, $"{preset.Name}{PresetExt}");
        ProtobufSerializer.Serialize(presetOutputFile, preset);

        // Add preset if created in presets folder.
        if (outputFile == null)
        {
            this.List.Add(preset);
        }
    }

    public void Create(Project project, string? outputFile = null)
    {
        var preset = new ProjectPreset
        {
            Name = project.Settings.Value.Name,
            Color = project.Settings.Value.Color,
            PostBuild = project.Settings.Value.PostBuild,
            DefaultTracks = project.Audio.Tracks.Select(x =>
                new AudioTrack
                {
                    Name = x.Name,
                    Category = x.Category,
                    Tags = x.Tags,
                    Encoder = x.Encoder,
                    OutputPath = x.OutputPath,
                })
            .ToArray(),
        };

        var projectIconFile = Path.Join(project.ProjectFolder, "icon.png");
        if (File.Exists(projectIconFile))
        {
            preset.Icon = File.ReadAllBytes(projectIconFile);
        }

        this.Create(preset, outputFile);
    }

    public void Delete(ProjectPreset preset)
    {
        var presetFile = Path.Join(this.presetsDir, $"{preset.Name}{PresetExt}");
        if (File.Exists(presetFile))
        {
            File.Delete(presetFile);
        }

        this.List.Remove(preset);
    }

    public ProjectPreset? GetById(string id)
    {
        return this.List.FirstOrDefault(x => x.Name == id);
    }

    private void LoadPresets()
    {
        foreach (var file in Directory.EnumerateFiles(this.presetsDir, $"*{PresetExt}", SearchOption.AllDirectories))
        {
            try
            {
                var preset = ProtobufSerializer.Deserialize<ProjectPreset>(file) ?? throw new Exception();
                this.List.Add(preset);
            }
            catch (Exception ex)
            {
                this.log?.LogError(ex, "Failed to load project preset file.\nFile: {file}", file);
            }
        }
    }

    private void AddDefaultPresets()
    {
        foreach (var preset in GamePresets.DefaultPresets)
        {
            if (this.GetById(preset.Name) == null)
            {
                this.Create(preset);
                this.List.Add(preset);
            }
        }
    }
}
