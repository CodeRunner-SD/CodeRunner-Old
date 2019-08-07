using System;

namespace CodeRunner.Loggings
{
    public class LogScope
    {
        public LogScope(Logger inner, string name)
        {
            Inner = inner;
            Name = name;
        }

        private Logger Inner { get; }

        public string Name { get; }

        public LogScope CreateScope(string name)
        {
            return new LogScope(Inner, $"{Name}/{name}");
        }

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

        public void Fatal(Exception exception)
        {
            Inner.Contents.Add(new LogItem
            {
                Level = LogLevel.Fatal,
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

        public void Information(string content)
        {
            Inner.Contents.Add(new LogItem
            {
                Level = LogLevel.Information,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Fatal(string content)
        {
            Inner.Contents.Add(new LogItem
            {
                Level = LogLevel.Fatal,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }

        public void Debug(string content)
        {
            Inner.Contents.Add(new LogItem
            {
                Level = LogLevel.Debug,
                Content = content,
                Scope = Name,
                Time = DateTimeOffset.Now
            });
        }
    }
}
