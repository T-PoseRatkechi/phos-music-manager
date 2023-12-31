﻿namespace Phos.MusicManager.Desktop;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Desktop.Common;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Encoders;
using Phos.MusicManager.Library.Commands;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Common.Logging;
using Phos.MusicManager.Library.Projects;
using Phos.MusicManager.Library.ViewModels;
using Phos.MusicManager.Library.ViewModels.Music;
using Phos.MusicManager.Library.ViewModels.Projects;
using Phos.MusicManager.Library.ViewModels.Projects.Factories;
using Serilog;
using System;
using System.IO;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(s =>
        new MainWindowViewModel(
            s.GetRequiredService<DashboardViewModel>(),
            s.GetRequiredService<NotificationsViewModel>(),
            s.GetRequiredService<ProjectsNavigation>(),
            s.GetRequiredService<ProjectCommands>(),
            s.GetRequiredService<LoopService>(),
            s.GetRequiredService<ProjectExporter>(),
            s.GetRequiredService<RefreshFilesCommand>(),
            s.GetRequiredService<ClearCacheCommand>(),
            s.GetRequiredService<AddFileCommand>(),
            s.GetRequiredService<IDialogService>(),
            s.GetRequiredService<Microsoft.Extensions.Logging.ILogger>())
        );

        serviceCollection.AddSingleton<DashboardViewModel>();
        serviceCollection.AddSingleton<NotificationsViewModel>();
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
        serviceCollection.AddSingleton<IDialogService, DialogService>();
        serviceCollection.AddSingleton<AudioEncoderRegistry>();
        serviceCollection.AddSingleton<MusicFactory>();
        serviceCollection.AddSingleton<AudioBuilder>();
        serviceCollection.AddSingleton<LoopService>();

        serviceCollection.AddSingleton<ProjectRepository>();
        serviceCollection.AddSingleton<ProjectPresetRepository>();
        serviceCollection.AddSingleton<CreateProjectFactory>();
        serviceCollection.AddSingleton<ProjectFactory>();
        serviceCollection.AddSingleton<ProjectsNavigation>();
        serviceCollection.AddSingleton<ProjectExporter>();
        serviceCollection.AddSingleton<PortableProjectConverter>();

        // Commands
        serviceCollection.AddSingleton<ProjectCommands>();
        serviceCollection.AddSingleton<CreatePresetCommand>();
        serviceCollection.AddSingleton<RefreshFilesCommand>();
        serviceCollection.AddSingleton<ClearCacheCommand>();
        serviceCollection.AddSingleton<AddFileCommand>();
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

        var logSink = new LogNotifySink();
        serviceCollection.AddSingleton<ILogNotify>(logSink);
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(logFile, outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Sink(logSink)
            .CreateLogger();

        var log = LoggerFactory.Create(logger => logger.AddSerilog(Log.Logger)).CreateLogger("Logger");
        serviceCollection.AddSingleton(log);
        log.LogInformation("Ready.");

        return serviceCollection;
    }
}
