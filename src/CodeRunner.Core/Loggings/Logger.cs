using System.Collections.Generic;

namespace CodeRunner.Loggings
{
    public enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }

    public class Logger
    {
        public Logger(string name, LogLevel level, Logger? parent = null)
        {
            Name = name;
            Parent = parent;
            Level = level;
        }

        public LogLevel Level { get; set; }

        public string Name { get; }

        public Logger? Parent { get; }

        private List<LogItem> Contents { get; } = new List<LogItem>();

        public LogItem[] GetAll() => Contents.ToArray();

        public void Log(LogItem item)
        {
            if (item.Level >= Level)
            {
                Contents.Add(item);
                if (Parent != null)
                    Parent.Log(item);
            }
        }

        public LogScope CreateScope(string name)
        {
            return new LogScope($"{Name}/{name}", this);
        }

        public Logger CreateLogger(string name, LogLevel level)
        {
            return new Logger($"{Name}/{name}", level, this);
        }
    }
}
