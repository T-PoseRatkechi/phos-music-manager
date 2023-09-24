using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Common.Logging;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System.IO;

namespace Phos.MusicManager.Desktop.Common;

internal class LogNotifySink : ILogEventSink, ILogNotify
{
    private readonly ITextFormatter formatter = new MessageTemplateTextFormatter("[{Level:u3}] {Message:lj}{NewLine}", null);

    public event LogReceived? OnLogReceived;

    public void Emit(LogEvent logEvent)
    {
        var level = logEvent.Level.ToLogLevel();
        var message = new StringWriter();
        formatter.Format(logEvent, message);

        this.OnLogReceived?.Invoke(new(level, message.ToString()));
    }
}

public static class LogEventLevelExtensions
{
    public static LogLevel ToLogLevel(this LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => LogLevel.Information,
        };
    }
}
