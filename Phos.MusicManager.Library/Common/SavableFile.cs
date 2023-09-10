namespace Phos.MusicManager.Library.Common;

using Phos.MusicManager.Library.Common.Serializers;

/// <summary>
/// Savable file.
/// </summary>
/// <typeparam name="TValue">Value type.</typeparam>
public class SavableFile<TValue> : ISavable<TValue>
{
    private readonly string filePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="SavableFile{TValue}"/> class.
    /// </summary>
    /// <param name="filePath">File path.</param>
    public SavableFile(string filePath)
    {
        this.filePath = filePath;
        this.Value = this.GetCurrentValue();
    }

    /// <inheritdoc/>
    public TValue Value { get; set; }

    /// <inheritdoc/>
    public void Save()
    {
        JsonFileSerializer.Serialize(this.filePath, this.Value);
    }

    private TValue GetCurrentValue()
    {
        if (!File.Exists(this.filePath))
        {
            return this.CreateDefault();
        }

        var currentValue = JsonFileSerializer.Deserialize<TValue>(this.filePath);
        return currentValue ?? throw new ArgumentException($"Failed to load file.\nFile: {this.filePath}");
    }

    private TValue CreateDefault()
    {
        var defaultValue = Activator.CreateInstance<TValue>();
        JsonFileSerializer.Serialize(this.filePath, defaultValue);

        return defaultValue;
    }
}
