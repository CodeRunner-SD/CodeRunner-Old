using System.Collections.Generic;

namespace CodeRunner.Loggings
{
    public interface ILogger
    {
        ILogger? Parent { get; }

        void Log(LogItem item);

        ILogger UseFilter(LogFilter filter);

        IEnumerable<LogItem> View();
    }
}
