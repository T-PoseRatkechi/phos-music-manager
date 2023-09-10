namespace Phos.MusicManager.Library;

#pragma warning disable SA1600 // Elements should be documented
public class AppSettings
{
    public string CurrentGame { get; set; } = Constants.P4G_PC_64;

    public bool DebugEnabled { get; set; }
}
