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
            viewModel.Closing += this.ViewModel_Closing;
        }
    }

    private void ViewModel_Closing(object? sender, object? e)
    {
        if (this.DataContext is WindowViewModelBase viewModel)
        {
            viewModel.Closing -= this.ViewModel_Closing;
        }

        this.Close(e);
    }
}
