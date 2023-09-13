namespace Phos.MusicManager.Library.ViewModels;

using Phos.MusicManager.Desktop.Library.ViewModels;

/// <summary>
/// Window view model base.
/// </summary>
public class WindowViewModelBase : ViewModelBase
{
    /// <summary>
    /// Window closing event.
    /// </summary>
    public event EventHandler<object?>? Closing;

    /// <summary>
    /// Close window.
    /// </summary>
    /// <param name="result">Dialog result.</param>
    public void Close(object? result = null)
    {
        this.Closing?.Invoke(this, result);
    }
}
