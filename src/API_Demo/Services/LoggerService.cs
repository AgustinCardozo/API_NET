using Microsoft.Extensions.Logging;

namespace API_Demo.Services
{
    public static class LoggerService
    {
        public static ILogger GetLogger()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
                builder.AddEventSourceLogger();
            });
            return loggerFactory.CreateLogger("Startup");
        }
    }
}
