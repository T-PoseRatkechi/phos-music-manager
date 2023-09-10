namespace Phos.MusicManager.Desktop.Library.ViewModels;

/// <summary>
/// Main window view model.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    /// <param name="rootViewModel">Root view model.</param>
    public MainWindowViewModel(ViewModelBase rootViewModel)
    {
        this.RootViewModel = rootViewModel;
    }

    /// <summary>
    /// Gets or sets the root view model.
    /// </summary>
    public ViewModelBase RootViewModel { get; set; }
}