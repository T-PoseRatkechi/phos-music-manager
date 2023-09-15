namespace Phos.MusicManager.Library.Navigation;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

/// <summary>
/// Navigation service.
/// </summary>
#pragma warning disable SA1600 // Elements should be documented
public partial class NavigationService : ObservableObject, INavigationService
{
    private readonly ILogger? log;

    [ObservableProperty]
    private IPage? current;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="log"></param>
    public NavigationService(ILogger? log = null)
    {
        this.log = log;
        this.Pages = new ObservableCollection<IPage>();
    }

    public IList<IPage> Pages { get; }

    /// <inheritdoc/>
    public void NavigateTo(string name)
    {
        var newPage = this.Pages.FirstOrDefault(pages => pages.Name == name);
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
        var newPage = this.Pages.FirstOrDefault(x => x.GetType() == typeof(TPage));
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
