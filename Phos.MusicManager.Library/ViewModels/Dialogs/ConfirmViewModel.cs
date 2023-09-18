namespace Phos.MusicManager.Library.ViewModels.Dialogs;

using CommunityToolkit.Mvvm.Input;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class ConfirmViewModel : WindowViewModelBase
{
    public string Title { get; set; } = "Confirm";

    public string? Subtitle { get; set; }

    public string? BodyText { get; set; }

    public string ConfirmText { get; set; } = "Confirm";

    public string CancelText { get; set; } = "Cancel";

    [RelayCommand]
    public override void Close(object? result = null)
    {
        base.Close(result ?? false);
    }

    [RelayCommand]
    private void Confirm()
    {
        this.Close(true);
    }
}
