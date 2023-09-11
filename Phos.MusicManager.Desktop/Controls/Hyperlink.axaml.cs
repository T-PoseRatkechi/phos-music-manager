using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Serilog;
using System.Diagnostics;
using System;
using Avalonia.Interactivity;

namespace Phos.MusicManager.Desktop.Controls;

public class Hyperlink : TemplatedControl
{
    public static readonly DirectProperty<Hyperlink, string?> UrlProperty =
        AvaloniaProperty.RegisterDirect<Hyperlink, string?>(nameof(UrlProperty), o => o.Url, (o, v) => o.Url = v);

    public static readonly DirectProperty<Hyperlink, string?> AliasProperty =
        AvaloniaProperty.RegisterDirect<Hyperlink, string?>(nameof(AliasProperty), o => o.Alias, (o, v) => o.Alias = v);

    private string? url;
    private string? alias;

    public Hyperlink()
    {
        this.Tapped += this.Hyperlink_Tapped;
    }

    public string? Url
    {
        get => this.url;
        set => this.SetAndRaise(UrlProperty, ref this.url, value);
    }

    public string? Alias
    {
        get => this.alias ?? this.Url ?? "URL missing.";
        set => this.SetAndRaise(AliasProperty, ref this.alias, value);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        this.Tapped -= this.Hyperlink_Tapped;
        base.OnUnloaded(e);
    }

    private void Hyperlink_Tapped(object? sender, TappedEventArgs e)
    {
        if (this.Url == null)
        {
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo() { FileName = this.url, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to open link.");
        }
    }
}