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
        public List<LogItem> Contents { get; } = new List<LogItem>();

        public LogScope CreateScope(string name)
        {
            return new LogScope(this, name);
        }
    }
}
