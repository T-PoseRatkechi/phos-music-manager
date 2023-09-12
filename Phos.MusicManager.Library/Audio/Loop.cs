namespace Phos.MusicManager.Library.Audio;

#pragma warning disable SA1600 // Elements should be documented
public class Loop
{
    public bool Enabled { get; set; }

    public int StartSample { get; set; }

    public int EndSample { get; set; }
}
