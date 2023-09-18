namespace Phos.MusicManager.Library.Audio.Encoders;

using Phos.MusicManager.Library.Audio.Models;

/// <summary>
/// Encoder interface.
/// </summary>
public interface IEncoder
{
    /// <summary>
    /// Gets default file extension of encoded file.
    /// </summary>
    string EncodedExt { get; }

    /// <summary>
    /// Gets list of valid input formats by extension.
    /// </summary>
    string[] InputTypes { get; }

    /// <summary>
    /// Encode file.
    /// </summary>
    /// <param name="inputFile">Input file.</param>
    /// <param name="outputFile">Output file.</param>
    /// <param name="loop">Loop settings.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Encode(string inputFile, string outputFile, Loop? loop = null);
}
