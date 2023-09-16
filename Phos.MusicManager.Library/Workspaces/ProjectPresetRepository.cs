namespace Phos.MusicManager.Library.Workspaces;

using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Common.Serializers;

#pragma warning disable SA1600 // Elements should be documented
public class ProjectPresetRepository : IRepository<ProjectPreset, string>
{
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

    public void Create(ProjectPreset preset)
    {
        if (this.GetById(preset.Name) != null)
        {
            this.Update(preset);
            return;
        }

        var presetFile = Path.Join(this.presetsDir, $"{preset.Name}.json");
        if (File.Exists(presetFile))
        {
            throw new Exception("Project preset file with same name already exists.");
        }

        JsonFileSerializer.Serialize(presetFile, preset);
        this.List.Add(preset);
    }

    public bool Update(ProjectPreset preset)
    {
        var currentPreset = this.GetById(preset.Name);
        if (currentPreset == null)
        {
            this.log?.LogError("Failed to find preset with name {name}.", preset.Name);
            return false;
        }

        currentPreset.Name = preset.Name;
        currentPreset.Color = preset.Color;
        currentPreset.DefaultTracks = preset.DefaultTracks;
        currentPreset.PostBuild = preset.PostBuild;

        var presetFile = Path.Join(this.presetsDir, $"{preset.Name}.json");
        JsonFileSerializer.Serialize(presetFile, currentPreset);

        return true;
    }

    public void Delete(ProjectPreset preset)
    {
        var presetFile = Path.Join(this.presetsDir, $"{preset.Name}.json");
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
        foreach (var file in Directory.EnumerateFiles(this.presetsDir, "*.json", SearchOption.AllDirectories))
        {
            try
            {
                var preset = JsonFileSerializer.Deserialize<ProjectPreset>(file) ?? throw new Exception();
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
            }
        }
    }
}
