using Microsoft.Extensions.Logging;
using Phos.MusicManager.Library.Common.Logging;
using System.Collections.Generic;

namespace Phos.MusicManager.Desktop.DesignTime;

public class MockNotifications
{
    public List<LogMessage> Messages { get; set; } = new()
    {
        new(LogLevel.Trace, "Trace."),
        new(LogLevel.Debug, "Debug."),
        new(LogLevel.Information, "Information."),
        new(LogLevel.Warning, "Warning."),
        new(LogLevel.Error, "Error."),
        new(LogLevel.Critical, "Critical."),
    };
}
