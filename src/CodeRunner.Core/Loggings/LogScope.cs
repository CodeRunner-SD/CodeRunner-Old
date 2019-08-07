using System;

namespace CodeRunner.Loggings
{
    public class LogScope
    {
        public LogScope(string name, Logger source)
        {
            Source = source;
            Name = name;
        }

        private Logger Source { get; }

        public string Name { get; }

        public LogScope CreateScope(string name)
        {
            return new LogScope($"{Name}/{name}", Source);
        }

        public Logger CreateLogger(string name, LogLevel level)
        {
            return new Logger($"{Name}/{name}", level, Source);
        }

        public void Error(Exception exception)
        {
            Source.Log(new LogItem
            {
                Level = LogLevel.Error,
                Content = exception.ToString(),
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Fatal(Exception exception)
        {
            Source.Log(new LogItem
            {
                Level = LogLevel.Fatal,
                Content = exception.ToString(),
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Error(string content)
        {
            Source.Log(new LogItem
            {
                Level = LogLevel.Error,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Warning(string content)
        {
            Source.Log(new LogItem
            {
                Level = LogLevel.Warning,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Information(string content)
        {
            Source.Log(new LogItem
            {
                Level = LogLevel.Information,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Fatal(string content)
        {
            Source.Log(new LogItem
            {
                Level = LogLevel.Fatal,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Debug(string content)
        {
            Source.Log(new LogItem
            {
                Level = LogLevel.Debug,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }
    }
}
