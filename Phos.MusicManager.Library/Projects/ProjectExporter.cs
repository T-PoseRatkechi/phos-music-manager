namespace Phos.MusicManager.Library.Projects;

using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Audio.Models;
using Phos.MusicManager.Library.Common.Serializers;

#pragma warning disable SA1600 // Elements should be documented
public class ProjectExporter
{
    private readonly ProjectPresetRepository presetRepository;
    private readonly AudioEncoderRegistry encoderRegistry;
    private readonly LoopService loop;

    public ProjectExporter(
        ProjectPresetRepository presetRepository,
        AudioEncoderRegistry encoderRegistry,
        LoopService loop)
    {
        this.presetRepository = presetRepository;
        this.encoderRegistry = encoderRegistry;
        this.loop = loop;
    }

    public void ExportProjectPreset(Project project, string outputFile)
    {
        this.presetRepository.Create(project, outputFile);
    }

    public void ExportPortableProject(Project project, string outputFile)
    {
        var portableProjectDir = Path.GetDirectoryName(outputFile) ?? throw new Exception("Failed to get output folder.");
        var projectSettings = project.Settings.Value;
        var portableProjectSettings = new ProjectSettings
        {
            Type = ProjectType.Portable1,
            Id = projectSettings.Id,
            Name = projectSettings.Name,
            Color = projectSettings.Color,
            PostBuild = projectSettings.PostBuild,
            Preset = projectSettings.Preset,
            Theme = projectSettings.Theme,
        };

        var portableAudioDir = Directory.CreateDirectory(Path.Join(portableProjectDir, "audio")).FullName;
        var portableEncodersDir = Directory.CreateDirectory(Path.Join(portableProjectDir, "encoders")).FullName;

        var uniqueReplacementFiles = project.Audio.Tracks.Where(x => x.ReplacementFile != null).Select(x => x.ReplacementFile!).ToHashSet();
        foreach (var replacementFile in uniqueReplacementFiles)
        {
            // Copy replacement file.
            var replacementFileName = Path.GetFileName(replacementFile);
            var outputReplacementFile = Path.Join(portableAudioDir, replacementFileName);
            File.Copy(replacementFile, outputReplacementFile, true);

            // Add loop file.
            var replacementFileLoop = this.loop.GetLoop(replacementFile);
            var outputLoopFile = $"{outputReplacementFile}.json";
            JsonFileSerializer.Serialize(outputLoopFile, replacementFileLoop);
        }

        List<AudioTrack> exportTracks = new();
        foreach (var track in project.Audio.Tracks)
        {
            // Add track with adjusted replacement file.
            exportTracks.Add(new()
            {
                Name = track.Name,
                Category = track.Category,
                Tags = track.Tags,
                Loop = track.Loop,
                Encoder = track.Encoder,
                OutputPath = track.OutputPath,
                ReplacementFile = Path.GetFileName(track.ReplacementFile),
            });

            // Copy encoders.
            if (track.Encoder != null)
            {
                if (this.encoderRegistry.EncoderFiles.TryGetValue(track.Encoder, out var encoderFile))
                {
                    var encoderRelativePath = Path.GetRelativePath(this.encoderRegistry.EncodersFolder, encoderFile);
                    var outputEncoderFile = Path.Join(portableEncodersDir, encoderRelativePath);
                    Directory.CreateDirectory(Path.GetDirectoryName(outputEncoderFile)!);
                    File.Copy(encoderFile, outputEncoderFile, true);
                }
            }
        }

        // Write audio tracks file.
        JsonFileSerializer.Serialize(Path.Join(portableAudioDir, "audio-tracks.json"), exportTracks);

        // Copy project icon.
        var iconFile = Path.Join(project.ProjectFolder, "icon.png");
        if (File.Exists(iconFile))
        {
            File.Copy(iconFile, Path.Join(portableProjectDir, "icon.png"), true);
        }

        // Write project file.
        JsonFileSerializer.Serialize(outputFile, portableProjectSettings);
    }
}
