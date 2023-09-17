namespace Phos.MusicManager.Library.Navigation;

using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Navigation service interface.
/// </summary>
public interface INavigationService : INotifyPropertyChanged
{
    /// <summary>
    /// Gets current page.
    /// </summary>
    IPage Current { get; set; }

    /// <summary>
    /// Gets available pages.
    /// </summary>
    ObservableCollection<IPage> Pages { get; }

    /// <summary>
    /// Navigate to specific page.
    /// </summary>
    /// <param name="name">Page name.</param>
    /// <returns>Value indicating navigation was successful.</returns>
    bool NavigateTo(string name);

    /// <summary>
    /// Navigate to specific page.
    /// </summary>
    /// <typeparam name="TPage">Page type.</typeparam>
    /// <returns>Value indicating navigation was successful.</returns>
    bool NavigateTo<TPage>()
        where TPage : IPage;
}
