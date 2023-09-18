namespace Phos.MusicManager.Library.Projects;

using Phos.MusicManager.Library.Audio.Encoders;

#pragma warning disable SA1600 // Elements should be documented
public class PortableProjectConverter
{
    private readonly AudioEncoderRegistry encoderRegistry;

    public PortableProjectConverter(AudioEncoderRegistry encoderRegistry)
    {
        this.encoderRegistry = encoderRegistry;
    }

    /// <summary>
    /// Converts a portable project into a regular project.
    /// </summary>
    /// <param name="project">Portable project to convert.</param>
    /// <returns>The converted project.</returns>
    public Project Convert(Project project)
    {
        if (project.Settings.Value.Type != ProjectType.Portable1)
        {
            return project;
        }

        // Connect replacement files.
        foreach (var track in project.Audio.Tracks)
        {
            if (track.ReplacementFile != null)
            {
                var newReplacementFile = Path.Join(project.AudioFolder, track.ReplacementFile);
                if (!File.Exists(newReplacementFile))
                {
                    throw new FileNotFoundException("Replacement file not found.", newReplacementFile);
                }

                track.ReplacementFile = newReplacementFile;
            }
        }

        // Add encoder files.
        var encodersDir = Path.Join(project.ProjectFolder, "encoders");
        if (Directory.Exists(encodersDir))
        {
            foreach (var encoderFile in Directory.EnumerateFiles(encodersDir, "*.*", SearchOption.AllDirectories))
            {
                var relativeEncoderFile = Path.GetRelativePath(encodersDir, encoderFile);
                var outputEncoderFile = Path.Join(this.encoderRegistry.EncodersFolder, relativeEncoderFile);
                if (!File.Exists(outputEncoderFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputEncoderFile)!);
                    File.Copy(encoderFile, outputEncoderFile, true);
                }
            }

            // Reload encoders.
            this.encoderRegistry.LoadEncoders();
        }

        project.Settings.Value.Type = ProjectType.Version1;
        project.Audio.SaveTracks();
        project.Settings.Save();
        return project;
    }
}
