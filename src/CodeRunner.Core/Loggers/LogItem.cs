using System;

namespace CodeRunner.Loggers
{
    public class LogItem
    {
        public LogLevel Level { get; set; }

        public string Scope { get; set; } = "";

        public DateTimeOffset Time { get; set; }

        public string Content { get; set; } = "";
    }
}
