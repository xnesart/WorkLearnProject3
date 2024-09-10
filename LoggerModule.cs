using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class LoggerModule : IMiddleware
{
    private readonly string _prefix;
    private readonly ILogger<LoggerModule> _logger;

    public LoggerModule(string prefix, ILogger<LoggerModule> logger)
    {
        _prefix = prefix ?? throw new ArgumentException("prefix can't be null or empty");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            var logMessageBefore = $"{_prefix} [{DateTime.Now}] Request: {context.Request.Path}";
            _logger.LogInformation(logMessageBefore);
            WriteToFile(logMessageBefore);

            await next(context);

            var logMessageAfter = $"{_prefix} [{DateTime.Now}] Response: {context.Response.StatusCode}";

            if (context.Items.ContainsKey("LogMessage"))
            {
                logMessageAfter += $"; Additional log: {context.Items["LogMessage"]}";
            }

            _logger.LogInformation(logMessageAfter);
            WriteToFile(logMessageAfter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logging");
        }
    }

    
    private void WriteToFile(string textToWrite)
    {
        try
        {
            using (var writer = new StreamWriter("log.txt", true)) 
            {
                var logEntry = $"{DateTime.Now}: {textToWrite}";
                writer.WriteLine(logEntry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
        }
    }
}