using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;

public class ConsoleLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new ConsoleLogger(name));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}

public class ConsoleLogger : ILogger
{
    private readonly string _categoryName;

    public ConsoleLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var level = logLevel.ToString().ToUpper().PadRight(5);
        var className = _categoryName.Split('.').LastOrDefault() ?? "Unknown";
        var message = formatter(state, exception);

        var logEntry = $"{timestamp} [{level}] {className}: {message}";

        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = GetLogLevelColor(logLevel);

        Console.WriteLine(logEntry);

        if (exception != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.ToString());
        }

        Console.ForegroundColor = originalColor;
    }

    private static ConsoleColor GetLogLevelColor(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Critical => ConsoleColor.Magenta,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Information => ConsoleColor.White,
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Trace => ConsoleColor.DarkGray,
            _ => ConsoleColor.White
        };
    }
}