namespace Phos.MusicManager.Library.Common.Logging;

#pragma warning disable SA1600 // Elements should be documented
public interface ILogNotify
{
    event LogReceived? OnLogReceived;
}
