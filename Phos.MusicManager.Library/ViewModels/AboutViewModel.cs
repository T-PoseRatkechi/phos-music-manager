﻿namespace Phos.MusicManager.Library.ViewModels;

using Phos.MusicManager.Desktop.Library.ViewModels;
using Phos.MusicManager.Library.Navigation;

#pragma warning disable SA1600 // Elements should be documented
public class AboutViewModel : ViewModelBase, IPage
{
    public string Name { get; } = "About";

    public Credit[] Credits { get; } = new Credit[]
    {
        new("T-PoseRatkechi", "Creator"),
        new("Thealexbarney", "VGAudio, core audio encoder.", "https://github.com/Thealexbarney/VGAudio"),
        new("Pixelguin", "Provided Persona 4 Golden 64-bit music data and song categories.", "https://gamebanana.com/members/1736439"),
        new("aray03", "Source for Persona 5 Royal PC music data.", "https://gamebanana.com/members/2323791"),
        new("ARandomGuy231 + Various Modders", "Source for Persona 3 Portable music data.", "https://gamebanana.com/members/1742788"),
        new("Lorc, Delapouite & contributors", "App icon. CC BY 3.0.", "https://game-icons.net"),
    };

    public record Credit(string Name, string Description, string? Url = null);
}
