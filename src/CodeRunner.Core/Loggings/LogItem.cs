using System;

namespace CodeRunner.Loggings
{
    public class LogItem
    {
        public LogLevel Level { get; set; }

        public string Scope { get; set; } = "";

        public DateTimeOffset Time { get; set; }

        public string Content { get; set; } = "";
    }
}
