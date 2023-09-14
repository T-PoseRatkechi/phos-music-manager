using Avalonia.Controls;
using Phos.MusicManager.Library.ViewModels;
using System;

namespace Phos.MusicManager.Desktop.Controls;

public class AppWindow : Window
{
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        if (this.DataContext is WindowViewModelBase viewModel)
        {
            // Connect to close from view.
            this.Closing += View_Closing;

            // Connect to close from view model.
            viewModel.Closing += this.ViewModel_Closing;
        }
    }

    /// <summary>
    /// Handles closing from view.
    /// </summary>
    private void View_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (this.DataContext is WindowViewModelBase viewModel)
        {
            // Unregister event listeners.
            this.Closing -= this.View_Closing;
            viewModel.Closing -= this.ViewModel_Closing;

            // Run vm close function.
            viewModel.Close();
        }

        this.Close();
    }

    /// <summary>
    /// Handles closing from view model.
    /// </summary>
    private void ViewModel_Closing(object? sender, object? e)
    {
        if (this.DataContext is WindowViewModelBase viewModel)
        {
            // Unregister event listeners.
            this.Closing -= this.View_Closing;
            viewModel.Closing -= this.ViewModel_Closing;
        }

        this.Close(e);
    }
}
