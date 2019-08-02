using System;

namespace CodeRunner.Loggers
{
    public class LogScope
    {
        public LogScope(Logger inner, string name)
        {
            Inner = inner;
            Name = name;
        }

        Logger Inner { get; }

        public string Name { get; }

        public void Error(Exception exception)
        {
            Inner.Contents.Add(new LogItem
            {
                Level = LogLevel.Error,
                Content = exception.ToString(),
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Error(string content)
        {
            Inner.Contents.Add(new LogItem
            {
                Level = LogLevel.Error,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Warning(string content)
        {
            Inner.Contents.Add(new LogItem
            {
                Level = LogLevel.Warning,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Info(string content)
        {
            Inner.Contents.Add(new LogItem
            {
                Level = LogLevel.Info,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }
    }
}
