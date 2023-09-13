namespace Phos.MusicManager.Desktop;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Desktop.Common;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Games;
using Phos.MusicManager.Library.Navigation;
using Phos.MusicManager.Library.ViewModels;
using Phos.MusicManager.Library.ViewModels.Music;
using Serilog;
using System;
using System.IO;
using System.Linq;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(s => new MainWindowViewModel(s.GetRequiredService<DashboardViewModel>()));
        serviceCollection.AddSingleton(s =>
        {
            var settings = s.GetRequiredService<ISavable<AppSettings>>();

            /* FluentAvalonia NavigationView breaks if left as IEnumerable. */
            var gameMenuItems = s.GetRequiredService<IGameService>().Games
                .Select(x =>new GameHubViewModel(x, s.GetRequiredService<MusicFactory>(), s.GetRequiredService<IDialogService>()))
                .ToArray();

            var appMenuItems = new IPage[]
            {
                s.GetRequiredService<SettingsViewModel>(),
                s.GetRequiredService<AboutViewModel>(),
            };

            return new DashboardViewModel(settings, gameMenuItems, appMenuItems);
        });

        serviceCollection.AddSingleton<SettingsViewModel>();
        serviceCollection.AddSingleton<AboutViewModel>();
        return serviceCollection;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
    {
        var settingsFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        try
        {
            var settings = new SavableFile<AppSettings>(settingsFile);
            serviceCollection.AddSingleton<ISavable<AppSettings>>(settings);
        }
        catch (Exception ex)
        {
            throw new NotImplementedException("To do.", ex);
        }

        return serviceCollection;
    }

    public static IServiceCollection AddLibrary(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IGameFactory, GameFactory>();
        serviceCollection.AddSingleton<IGameService, GameService>();
        serviceCollection.AddSingleton<IDialogService, DialogService>();
        serviceCollection.AddSingleton<AudioEncoderRegistry>();
        serviceCollection.AddSingleton<MusicFactory>();
        return serviceCollection;
    }

    public static IServiceCollection AddLogging(this IServiceCollection serviceCollection)
    {
        var logFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
        try
        {
            File.Delete(logFile);
        }
        catch (Exception) { }

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(logFile, outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        var log = LoggerFactory.Create(logger => logger.AddSerilog(Log.Logger)).CreateLogger("Logger");
        serviceCollection.AddSingleton(log);
        log.LogInformation("Ready.");

        return serviceCollection;
    }
}
