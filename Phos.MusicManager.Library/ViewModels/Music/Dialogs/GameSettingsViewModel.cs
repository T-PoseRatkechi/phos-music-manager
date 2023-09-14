﻿namespace Phos.MusicManager.Library.ViewModels.Music.Dialogs;

using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.Games;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class GameSettingsViewModel : WindowViewModelBase
{
    private readonly IDialogService dialog;
    private readonly ISavable<GameSettings> settings;

    public GameSettingsViewModel(ISavable<GameSettings> settings, IDialogService dialog)
    {
        this.dialog = dialog;
        this.settings = settings;
        this.settings.Value.PropertyChanged += this.Settings_PropertyChanged;
    }

    public GameSettings Settings => this.settings.Value;

    [RelayCommand]
    public override void Close(object? result = null)
    {
        this.settings.Value.PropertyChanged -= this.Settings_PropertyChanged;
        base.Close(result);
    }

    [RelayCommand]
    private async Task SelectOutputFolder()
    {
        var outputDir = await this.dialog.OpenFolderSelect("Select Output Folder...");
        if (!string.IsNullOrEmpty(outputDir))
        {
            this.settings.Value.OutputDir = outputDir;
        }
    }

    [RelayCommand]
    private async Task SelectInstallPath()
    {
        var installPath = await this.dialog.OpenFileSelect("Select Game Install...");
        if (!string.IsNullOrEmpty(installPath))
        {
            this.settings.Value.InstallPath = installPath;
        }
    }

    private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.settings.Save();
    }
}
