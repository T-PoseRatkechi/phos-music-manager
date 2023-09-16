namespace Phos.MusicManager.Library.Common;

using System.Collections.ObjectModel;

/// <summary>
/// Repository interface.
/// </summary>
/// <typeparam name="T">Items type.</typeparam>
/// <typeparam name="TId">ID type.</typeparam>
public interface IRepository<T, TId>
    where T : class
{
    /// <summary>
    /// Gets list of all items.
    /// </summary>
    ObservableCollection<T> List { get; }

    /// <summary>
    /// Creates a new item.
    /// </summary>
    /// <param name="item">Item to create.</param>
    void Create(T item);

    /// <summary>
    /// Deletes an item.
    /// </summary>
    /// <param name="item">Item to delete.</param>
    void Delete(T item);

    /// <summary>
    /// Gets item by id.
    /// </summary>
    /// <param name="id">ID of item.</param>
    /// <returns>Item with matching ID.</returns>
    T? GetById(TId id);

    /// <summary>
    /// Updates an item.
    /// </summary>
    /// <param name="item">Item to update.</param>
    /// <returns>Value indicating success of update.</returns>
    bool Update(T item);
}