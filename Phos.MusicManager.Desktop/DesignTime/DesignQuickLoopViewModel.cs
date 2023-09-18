using Phos.MusicManager.Library.ViewModels.Music;
using System.Collections.Generic;

namespace Phos.MusicManager.Desktop.DesignTime;

public class DesignQuickLoopViewModel
{
    public DesignQuickLoopViewModel()
    {
        this.FilteredItems = new()
        {
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
            new QuickLoopItem() { FileName = "Name", File = "file.wav" },
        };
    }

    public string Filter { get; set; } = string.Empty;

    public List<QuickLoopItem> FilteredItems { get; set; }
}
