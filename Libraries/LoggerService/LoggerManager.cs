using NLog;

namespace LoggerService;

public class LoggerManager : ILoggerManager
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public LoggerManager() { }

    public void LogInfo(string message) => _logger.Info(message);

    public void LogWarning(string message) => _logger.Warn(message);

    public void LogDebug(string message) => _logger.Debug(message);

    public void LogError(string message) => _logger.Error(message);

    public void LogError(Exception exception, string message) => _logger.Error(exception, message);
}
