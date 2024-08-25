using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRCQRS.Application.Events.Handlers
{
  // LoggingEventHandler.cs
  public class LoggingEventHandler : INotificationHandler<LoggingEvent>
  {
    private readonly ILogger<LoggingEventHandler> _logger;
    private readonly CRCQRSContext _context;

    public LoggingEventHandler(ILogger<LoggingEventHandler> logger, CRCQRSContext context)
    {
      _logger = logger;
      _context = context;
    }

    public async Task Handle(LoggingEvent notification, CancellationToken cancellationToken)
    {
      // Log to console or other logging frameworks
      _logger.Log(
          Enum.Parse<LogLevel>(notification.LogLevel),
          notification.Message
      );

      // Optional: Log to database
      var logEntry = new LogEntry
      {
        LogLevel = notification.LogLevel,
        Message = notification.Message,
        Timestamp = notification.Timestamp,
        UserId = notification.UserId,
        Type = notification.Source.GetType().FullName,
        Source = Utility.ConvertObjectToJsonString(notification.Source)
      };

      _context.LogEntries.Add(logEntry);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }

}
