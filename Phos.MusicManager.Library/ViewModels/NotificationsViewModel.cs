namespace Phos.MusicManager.Library.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Common.Logging;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class NotificationsViewModel : ViewModelBase
{
    public NotificationsViewModel(ILogNotify notify)
    {
        notify.OnLogReceived += this.Notify_OnLogReceived;
    }

    public ObservableCollection<LogMessage> Messages { get; } = new();

    [RelayCommand]
    private void CloseMessage(LogMessage message)
    {
        this.Messages.Remove(message);
    }

    private void Notify_OnLogReceived(LogMessage message)
    {
        if (message.Level >= LogLevel.Error)
        {
            message.Message = $"{message.Message}See log file for more information.";
        }

        this.Messages.Add(message);
    }
}
