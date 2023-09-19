using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library;
using System;

namespace Phos.MusicManager.Desktop;

/// <summary>
/// View locator.
/// </summary>
public class ViewLocator : IDataTemplate
{
    private static readonly string ViewBaseNamespace = typeof(App).Namespace!;
    private static readonly string ViewModelBaseNamespace = typeof(AppSettings).Namespace!;

    /// <inheritdoc/>
    public Control Build(object? data)
    {
        if (data == null)
        {
            return new TextBlock { Text = $"ViewLocator: {nameof(data)} was null." };
        }

        try
        {
            var dataType = data.GetType();
            var viewFullname = dataType.FullName!.Replace(ViewModelBaseNamespace, ViewBaseNamespace).Replace("ViewModel", "View");
            var viewType = Type.GetType(viewFullname);

            if (viewType != null)
            {
                var view = (Control)Activator.CreateInstance(viewType)!;
                view.DataContext = data;
                return view;
            }

            return new TextBlock { Text = "Not Found: " + viewFullname };
        }
        catch (Exception ex)
        {
            return new TextBlock { Text = $"Error: {ex.Message}" };
        }
    }

    /// <inheritdoc/>
    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}