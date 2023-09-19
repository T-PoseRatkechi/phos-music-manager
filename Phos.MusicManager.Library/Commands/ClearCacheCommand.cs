namespace Phos.MusicManager.Library.Commands;

using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Common;
using Phos.MusicManager.Library.ViewModels.Dialogs;

#pragma warning disable SA1600 // Elements should be documented
public class ClearCacheCommand
{
    private readonly ILogger? log;
    private readonly IDialogService dialog;

    public ClearCacheCommand(IDialogService dialog, ILogger? log = null)
    {
        this.log = log;
        this.dialog = dialog;
    }

    public IRelayCommand Create() =>
        new AsyncRelayCommand(this.ClearCache);

    private async Task ClearCache()
    {
        var cachedDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "audio", "cached");
        var confirmClearDialog = new ConfirmViewModel()
        {
            Title = "Clear Cache",
            Subtitle = cachedDir,
            BodyText = "Clear all files in cached folder?",
        };

        var confirmClear = await this.dialog.OpenDialog<bool>(confirmClearDialog);
        if (!confirmClear)
        {
            return;
        }

        try
        {
            foreach (var file in Directory.EnumerateFiles(cachedDir, "*.*", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
        }
        catch (Exception ex)
        {
            this.log?.LogError(ex, "Failed to clear cache.");
        }
    }
}
