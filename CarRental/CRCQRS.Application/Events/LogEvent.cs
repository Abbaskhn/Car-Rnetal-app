using MediatR;

namespace CRCQRS.Application.Events
{
  public class LoggingEvent : INotification
  {
    public string LogLevel { get; set; } // e.g., Info, Warning, Error
    public string Message { get; set; } // Log message
    public DateTime Timestamp { get; set; } // Time of the log event
    public long UserId { get; set; } // Optional: ID of the user triggering the event
    public object Source { get; set; } // Optional: Source of the log (e.g., application area)

    public LoggingEvent(string logLevel, string message, DateTime timestamp, long userId = 0, object source = null)
    {
      LogLevel = logLevel;
      Message = message;
      Timestamp = timestamp;
      UserId = userId;
      Source = source;
    }
  }

}
