namespace Phos.MusicManager.Desktop;

using Microsoft.Extensions.DependencyInjection;
using Phos.MusicManager.Desktop.Library.ViewModels;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MainWindowViewModel>();
        return serviceCollection;
    }
}
