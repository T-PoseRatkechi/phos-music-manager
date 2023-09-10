namespace Phos.MusicManager.Desktop;

using Microsoft.Extensions.DependencyInjection;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.ViewModels;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(s => new MainWindowViewModel(s.GetRequiredService<DashboardViewModel>()));
        serviceCollection.AddSingleton<DashboardViewModel>();
        return serviceCollection;
    }
}
