namespace Phos.MusicManager.Library.Common;

/// <summary>
/// Savable object interface.
/// </summary>
/// <typeparam name="TValue">Value type.</typeparam>
public interface ISavable<TValue>
{
    /// <summary>
    /// Gets or sets current value.
    /// </summary>
    TValue Value { get; set; }

    /// <summary>
    /// Saves current value.
    /// </summary>
    void Save();
}