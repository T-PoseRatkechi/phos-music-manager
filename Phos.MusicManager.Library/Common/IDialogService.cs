namespace Phos.MusicManager.Library.Common;

using Phos.MusicManager.Desktop.Library.ViewModels;

/// <summary>
/// Dialog service interface.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Opens a file select dialog.
    /// </summary>
    /// <param name="title">Window title.</param>
    /// <param name="filter">File filter.</param>
    /// <param name="initialDirectory">Initial directory.</param>
    /// <returns>The selected file.</returns>
    Task<string?> OpenFileSelect(string? title = null, string? filter = null, string? initialDirectory = null);

    /// <summary>
    /// Opens a multi file select dialog.
    /// </summary>
    /// <param name="title">Window title.</param>
    /// <param name="filter">File filter.</param>
    /// <param name="initialDirectory">Initial directory.</param>
    /// <returns>The selected files.</returns>
    Task<string[]?> OpenFilesSelect(string? title = null, string? filter = null, string? initialDirectory = null);

    /// <summary>
    /// Opens a folder select dialog.
    /// </summary>
    /// <param name="title">Window title.</param>
    /// <param name="initialDirectory">Initial directory.</param>
    /// <returns>The selected folder.</returns>
    Task<string?> OpenFolderSelect(string? title = null, string? initialDirectory = null);

    /// <summary>
    /// Opens a dialog.
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="dialog">Dialog view model.</param>
    /// <param name="owner">Owner view model.</param>
    /// <returns>Dialog result.</returns>
    Task<TResult?> OpenDialog<TResult>(ViewModelBase dialog, ViewModelBase? owner = null);
}
