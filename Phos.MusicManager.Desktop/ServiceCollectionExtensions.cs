namespace Phos.MusicManager.Desktop;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Desktop.Common;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library;
using Phos.MusicManager.Library.Audio;
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
            var audioBuilder = s.GetRequiredService<AudioBuilder>();
            var musicFactory = s.GetRequiredService<MusicFactory>();
            var dialog = s.GetRequiredService<IDialogService>();
            var log = s.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();

            var gamesFactory = s.GetRequiredService<IGameFactory>();

            var gamePages = gamesFactory.GetGames().Select(x => new GameHubViewModel(x, audioBuilder, musicFactory, dialog, log)).ToArray();
            var appPages = new IPage[]
            {
                s.GetRequiredService<SettingsViewModel>(),
                s.GetRequiredService<AboutViewModel>(),
            };

            var navigation = new NavigationService(gamePages.Concat(appPages), log);
            return new DashboardViewModel(settings, navigation, appPages.Select(x => x.Name).ToArray());
        });

        serviceCollection.AddSingleton<SettingsViewModel>();
        serviceCollection.AddSingleton<AboutViewModel>();
        return serviceCollection;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
    {
        var settingsFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        var settings = new SavableFile<AppSettings>(settingsFile);
        serviceCollection.AddSingleton<ISavable<AppSettings>>(settings);
        return serviceCollection;
    }

    public static IServiceCollection AddLibrary(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IGameFactory, GameFactory>();
        serviceCollection.AddSingleton<IDialogService, DialogService>();
        serviceCollection.AddSingleton<AudioEncoderRegistry>();
        serviceCollection.AddSingleton<MusicFactory>();
        serviceCollection.AddSingleton<AudioBuilder>();
        serviceCollection.AddSingleton<LoopService>();
        return serviceCollection;
    }

    public static IServiceCollection AddLogging(this IServiceCollection serviceCollection)
    {
        var logFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
        try
        {
            if (File.Exists(logFile))
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
