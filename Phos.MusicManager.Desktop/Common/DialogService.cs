using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phos.MusicManager.Desktop.Common;

public class DialogService : IDialogService
{
    private readonly ViewLocator viewLocator = new();

    public async Task<TResult?> OpenDialog<TResult>(ViewModelBase dialog, ViewModelBase? owner = null)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            && desktop.MainWindow is not null)
        {
            // Get vm view.
            var view = this.viewLocator.Build(dialog);

            // Use view as window if already window,
            // else create new window with view.
            var window = view as Window ?? new Window()
            {
                Content = view,
                Width = 600,
                MinWidth = 600,
                Height = 350,
                MinHeight = 350,
            };

            // Set view vm.
            window.DataContext = dialog;

            // Show dialog and get result.
            // TODO: Implement owner.
            // All dialogs use main window as owner.
            var result = await window.ShowDialog<TResult>(desktop.MainWindow);
            return result;
        }

        return default;
    }

    public async Task OpenDialog(ViewModelBase dialog, ViewModelBase? owner = null)
    {
        await this.OpenDialog<object?>(dialog, owner);
    }

    public async Task<string?> OpenFileSelect(string? title = null, string? filter = null, string? initialDirectory = null)
    {
        var filters = CreateFilterList(filter);
        var options = new FilePickerOpenOptions
        {
            Title = title ?? "Select File",
            AllowMultiple = false,
            FileTypeFilter = filters.Count > 0 ? filters : null,
        };

        var storageProvider = GetStorageProvider();
        if (storageProvider == null)
        {
            return null;
        }

        var storageFiles = await storageProvider.OpenFilePickerAsync(options);
        if (storageFiles == null || storageFiles.Count < 1)
        {
            return null;
        }

        return storageFiles[0].Path.LocalPath;
    }

    public async Task<string[]?> OpenFilesSelect(string? title = null, string? filter = null, string? initialDirectory = null)
    {
        var filters = CreateFilterList(filter);
        var options = new FilePickerOpenOptions
        {
            Title = title ?? "Select File(s)",
            AllowMultiple = true,
            FileTypeFilter = filters.Count > 0 ? filters : null,
        };

        var storageProvider = GetStorageProvider();
        if (storageProvider == null)
        {
            return null;
        }

        var storageFiles = await storageProvider.OpenFilePickerAsync(options);
        if (storageFiles == null || storageFiles.Count < 1)
        {
            return null;
        }

        return storageFiles.Select(x => x.Path.LocalPath).ToArray();
    }

    public async Task<string?> OpenFolderSelect(string? title = null, string? initialDirectory = null)
    {
        var options = new FolderPickerOpenOptions
        {
            Title = title ?? "Select Folder",
            AllowMultiple = false,
        };

        var storageProvider = GetStorageProvider();
        if (storageProvider == null)
        {
            return null;
        }

        var storageFolders = await storageProvider.OpenFolderPickerAsync(options);
        if (storageFolders == null || storageFolders.Count < 1)
        {
            return null;
        }

        return storageFolders[0].Path.LocalPath;
    }

    private static IReadOnlyList<FilePickerFileType> CreateFilterList(string? filter)
    {
        var filters = new List<FilePickerFileType>();
        if (filter != null)
        {
            try
            {
                var filterSplit = filter.Split('|');
                for (int i = 0; i < filterSplit.Length; i += 2)
                {
                    var filterName = filterSplit[0];
                    var filterTypes = filterSplit[i + 1].Split(';');
                    var newFilter = new FilePickerFileType(filterName)
                    {
                        Patterns = filterTypes,
                    };

                    filters.Add(newFilter);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create file filter list.");
            }
        }

        return filters;
    }

    private static IStorageProvider? GetStorageProvider()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow?.StorageProvider;
        }

        return null;
    }
}
