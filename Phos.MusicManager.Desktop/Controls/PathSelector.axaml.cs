using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace Phos.MusicManager.Desktop.Controls;

public enum PathType
{
    File,
    Folder,
}

public partial class PathSelector : UserControl
{
    public static readonly DirectProperty<PathSelector, PathType> TypeProperty =
        AvaloniaProperty.RegisterDirect<PathSelector, PathType>(nameof(Type), o => o.Type, (o, v) => o.Type = v);

    public static readonly DirectProperty<PathSelector, string?> TextProperty =
        AvaloniaProperty.RegisterDirect<PathSelector, string?>(nameof(Text), o => o.Text, (o, v) => o.Text = v);

    public static readonly DirectProperty<PathSelector, ICommand?> CommandProperty =
        AvaloniaProperty.RegisterDirect<PathSelector, ICommand?>(nameof(Command), o => o.Command, (o, v) => o.Command = v);

    private PathType type = PathType.File;
    private string? text;
    private ICommand? command;

    public PathSelector()
    {
        InitializeComponent();
        this.pathButton.Classes.Add(this.type == PathType.File ? "selectFileButton" : "selectFolderButton");
    }

    public PathType Type
    {
        get => this.type;
        set
        {
            this.SetAndRaise(TypeProperty, ref this.type, value);
            this.pathButton.Classes.Clear();
            this.pathButton.Classes.Add(this.type == PathType.File ? "selectFileButton" : "selectFolderButton");
        }
    }

    public string? Text
    {
        get => this.text;
        set => this.SetAndRaise(TextProperty, ref this.text, value);
    }

    public ICommand? Command
    {
        get => this.command;
        set => this.SetAndRaise(CommandProperty, ref this.command, value);
    }
}