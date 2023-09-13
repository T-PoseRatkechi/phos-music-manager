namespace Phos.MusicManager.Library.ViewModels.Music.Dialogs;

using CommunityToolkit.Mvvm.Input;
using Phos.MusicManager.Library.Audio.Models;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
public partial class AddTrackViewModel : WindowViewModelBase
{
    private string name = string.Empty;
    private string outputPath = string.Empty;

    public AddTrackViewModel(string[] encoders, AudioTrack? track = null)
    {
        this.Encoders = encoders;
        if (track != null)
        {
            this.name = track.Name;
            this.outputPath = track.OutputPath ?? string.Empty;

            this.Category = track.Category ?? string.Empty;
            this.Tags = string.Join(", ", track.Tags);
            this.SelectedEncoder = track.Encoder;
            this.IsEditing = true;
        }

        if (string.IsNullOrEmpty(this.SelectedEncoder) && this.Encoders.Length > 0)
        {
            this.SelectedEncoder = this.Encoders[0];
        }
    }

    public bool IsEditing { get; }

    public string Name
    {
        get => this.name;
        set
        {
            this.SetProperty(ref this.name, value);
            this.ConfirmCommand.NotifyCanExecuteChanged();
        }
    }

    public string Category { get; set; } = string.Empty;

    public string Tags { get; set; } = string.Empty;

    public string OutputPath
    {
        get => this.outputPath;
        set
        {
            var path = value.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            this.SetProperty(ref this.outputPath, path);
            this.ConfirmCommand.NotifyCanExecuteChanged();
        }
    }

    public string[] Encoders { get; }

    public string SelectedEncoder { get; set; } = string.Empty;

    public bool CanConfirm =>
        !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.OutputPath);

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm()
    {
        var tags = this.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
        var track = new AudioTrack()
        {
            Name = this.Name,
            Category = string.IsNullOrEmpty(this.Category) ? null : this.Category,
            Tags = tags.Length > 0 ? tags : Array.Empty<string>(),
            OutputPath = this.OutputPath,
            Encoder = this.SelectedEncoder,
        };

        this.Close(track);
    }

    [RelayCommand]
    private void Cancel()
    {
        this.Close();
    }
}
