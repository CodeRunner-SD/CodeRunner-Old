using CodeRunner.Diagnostics;

namespace CodeRunner.Loggings
{
    public static class LoggerExtensions
    {
        public static LoggerScope CreateScope(this ILogger logger, string name, LogLevel level)
        {
            Assert.IsNotNull(name);

            return new LoggerScope(logger, $"/{name}", level);
        }

        public static ILogger UseLevelFilter(this ILogger logger, LogLevel level)
        {
            Assert.IsNotNull(logger);

            return logger.UseFilter(LogFilter.Create(item => item.Level >= level));
        }
    }
}
