using Avalonia;
using Avalonia.Controls;

namespace Phos.MusicManager.Desktop.Controls;

public partial class Page : UserControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Page, string?>(nameof(Title));

    public static readonly DirectProperty<Page, object?> MenuItemsProperty =
        AvaloniaProperty.RegisterDirect<Page, object?>(nameof(MenuItems), o => o.MenuItems, (o, v) => o.MenuItems = v);

    private object? menuItems;

    public Page()
    {
        InitializeComponent();
    }

    public string? Title
    {
        get => this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    public object? MenuItems
    {
        get => this.menuItems;
        set => this.SetAndRaise(MenuItemsProperty, ref this.menuItems, value);
    }
}