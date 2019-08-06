using System.Collections.Generic;

namespace CodeRunner.Loggers
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public class Logger
    {
        public List<LogItem> Contents { get; } = new List<LogItem>();

        public LogScope CreateScope(string name)
        {
            return new LogScope(this, name);
        }
    }
}
