namespace Phos.MusicManager.Library.Navigation;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

/// <summary>
/// Navigation service.
/// </summary>
public partial class NavigationService : ObservableObject, INavigationService
{
    private readonly ILogger? log;
    private readonly IEnumerable<IPage> pages;

    [ObservableProperty]
    private IPage? current;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="pages">Pages available to navigate to.</param>
    /// <param name="log"></param>
    public NavigationService(IEnumerable<IPage> pages, ILogger? log = null)
    {
        this.log = log;
        this.pages = pages;
    }

    /// <inheritdoc/>
    public void NavigateTo(string name)
    {
        var newPage = this.pages.FirstOrDefault(pages => pages.Name == name);
        if (newPage != null)
        {
            this.Current = newPage;
        }
        else
        {
            this.log?.LogError("Page not found by name: {name}", name);
        }
    }

    /// <inheritdoc/>
    public void NavigateTo<TPage>()
        where TPage : IPage
    {
        var newPage = this.pages.FirstOrDefault(x => x.GetType() == typeof(TPage));
        if (newPage != null)
        {
            this.Current = newPage;
        }
        else
        {
            this.log?.LogError("Page not found by type: {type}", typeof(TPage).Name);
        }
    }
}
