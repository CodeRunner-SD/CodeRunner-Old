using System;

namespace CodeRunner.Loggings
{
    public class LoggerScope
    {
        public LoggerScope(ILogger source, string name, LogLevel level)
        {
            Source = source;
            Name = name;
            Level = level;
        }

        private ILogger Source { get; }

        public string Name { get; }

        public LogLevel Level { get; set; }

        public void Log(string content, LogLevel level)
        {
            if (level >= Level)
            {
                Source.Log(new LogItem
                {
                    Level = level,
                    Content = content,
                    Scope = Name,
                    Time = DateTimeOffset.Now
                });
            }
        }

        public void Error(Exception exception)
        {
            Error(exception.ToString());
        }

        public void Fatal(Exception exception)
        {
            Fatal(exception.ToString());
        }

        public void Error(string content)
        {
            Log(content, LogLevel.Error);
        }

        public void Warning(string content)
        {
            Log(content, LogLevel.Warning);
        }

        public void Information(string content)
        {
            Log(content, LogLevel.Information);
        }

        public void Fatal(string content)
        {
            Log(content, LogLevel.Fatal);
        }

        public void Debug(string content)
        {
            Log(content, LogLevel.Debug);
        }

        public LoggerScope CreateScope(string name, LogLevel level)
        {
            return new LoggerScope(Source, $"{Name}/{name}", level);
        }
    }
}
