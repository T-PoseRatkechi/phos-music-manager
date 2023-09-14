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
            // Connect to close from view model.
            viewModel.Closing += this.ViewModel_Closing;

            // Connect to close from view.
            this.Closing += this.ViewModel_Closing;
        }
    }

    private void ViewModel_Closing(object? sender, object? e)
    {
        if (this.DataContext is WindowViewModelBase viewModel)
        {
            // Run close function from vm.
            viewModel.Close();

            // Unregister event listeners.
            viewModel.Closing -= this.ViewModel_Closing;
            this.Closing -= this.ViewModel_Closing;
        }

        this.Close(e);
    }
}
