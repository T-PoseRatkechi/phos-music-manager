namespace Phos.MusicManager.Library.Common.Logging;

using Microsoft.Extensions.Logging;

#pragma warning disable SA1600 // Elements should be documented
public class LogMessage
{
    public LogMessage(LogLevel level, string message)
    {
        this.Level = level;
        this.Message = message;
    }

    public LogLevel Level { get; set; }

    public string Message { get; set; }
}
