namespace Phos.MusicManager.Library.Projects;

using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Common.Serializers;

#pragma warning disable SA1600 // Elements should be documented
public class ProjectExportService
{
    private readonly AudioEncoderRegistry encoderRegistry;
    private readonly LoopService loop;

    public ProjectExportService(AudioEncoderRegistry encoderRegistry, LoopService loop)
    {
        this.encoderRegistry = encoderRegistry;
        this.loop = loop;
    }

    public void ExportPortableProject(Project project, string outputFile)
    {
        var outputDir = Path.GetDirectoryName(outputFile) ?? throw new Exception("Failed to get output folder.");
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

        var portableAudioDir = Directory.CreateDirectory(Path.Join(outputDir, "audio")).FullName;
        var portableEncodersDir = Directory.CreateDirectory(Path.Join(outputDir, "encoders")).FullName;

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

        foreach (var track in project.Audio.Tracks)
        {
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

        JsonFileSerializer.Serialize(outputFile, portableProjectSettings);
    }
}
