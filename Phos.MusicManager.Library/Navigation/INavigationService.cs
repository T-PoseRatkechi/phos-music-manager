namespace Phos.MusicManager.Library.Navigation;

/// <summary>
/// Navigation service interface.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Gets current page.
    /// </summary>
    IPage Current { get; }

    /// <summary>
    /// Navigate to specific page.
    /// </summary>
    /// <param name="name">Page name.</param>
    void NavigateTo(string name);

    /// <summary>
    /// Navigate to specific page.
    /// </summary>
    /// <typeparam name="TPage">Page type.</typeparam>
    void NavigateTo<TPage>()
        where TPage : IPage;
}
