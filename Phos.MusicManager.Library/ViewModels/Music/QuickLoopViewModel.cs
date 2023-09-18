namespace Phos.MusicManager.Library.ViewModels.Music;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FuzzySharp;
using Phos.MusicManager.Library.Audio;
using Phos.MusicManager.Library.Audio.Models;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1402 // File may only contain a single type
public partial class QuickLoopViewModel : WindowViewModelBase
{
    private readonly string folderDir;
    private readonly LoopService loopService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredItems))]
    private string filter = string.Empty;

    public QuickLoopViewModel(string folderDir, LoopService loopService)
    {
        this.folderDir = folderDir;
        this.loopService = loopService;
        this.Items = this.GetItems();
    }

    public QuickLoopItem[] FilteredItems
    {
        get
        {
            return this.Items.Where(item =>
            {
                var itemSearchString = string.Join(' ', item.FileName, item.File).ToLower();
                return string.IsNullOrEmpty(this.Filter) || Fuzz.PartialRatio(this.Filter.ToLower(), itemSearchString) >= Math.Min(25 * this.Filter.Length, 100);
            }).ToArray();
        }
    }

    public List<QuickLoopItem> Items { get; }

    [RelayCommand]
    public override void Close(object? result = null)
    {
        base.Close(result);
    }

    [RelayCommand]
    private void Confirm()
    {
        foreach (var item in this.Items)
        {
            if (item.File != null)
            {
                this.loopService.SaveLoop(item.File, item.Loop, $"{item.File}.json");
            }
        }

        this.Close();
    }

    private List<QuickLoopItem> GetItems()
    {
        var items = new List<QuickLoopItem>();
        foreach (var file in Directory.EnumerateFiles(this.folderDir, "*.*", SearchOption.AllDirectories))
        {
            // Ignore existing loop files.
            if (Path.GetExtension(file).ToLower() == ".json")
            {
                continue;
            }

            var existingLoop = this.loopService.GetLoop(file);
            items.Add(new() { FileName = Path.GetFileName(file), File = file, Loop = existingLoop ?? new() { Enabled = true } });
        }

        return items;
    }
}

public class QuickLoopItem
{
    public string? FileName { get; set; }

    public string? File { get; set; }

    public Loop Loop { get; set; } = new();
}
