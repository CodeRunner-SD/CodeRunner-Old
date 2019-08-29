using System.Collections.Generic;

namespace CodeRunner.Loggings
{
    public interface ILogger
    {
        ILogger? Parent { get; }

        void Log(LogItem item,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0);

        ILogger UseFilter(LogFilter filter);

        IEnumerable<LogItem> View();
    }
}
